using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Transport_Service.Data;
using Transport_Service.Models.DTOs;
using Transport_Service.Models.Entities;

namespace Transport_Service.Controllers
{
    /// <summary>
    /// Controller that handles CRUD operations on table Transport
    /// </summary>
    [ApiController]
    [Route("api/transport")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TransportController
    {
        private readonly ApplicationDbContext context;
        private readonly Logger logger;
        private readonly IHttpContextAccessor contextAccessor;

        public TransportController(ApplicationDbContext context, Logger logger, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
        }

        [SwaggerOperation(summary: "Transports by provided weight", description: "Endpoint that returns all available transport types and their prices based on sent weight parameter")]
        [SwaggerResponse(200, "Returns available transports")]
        [SwaggerResponse(400, "Parameter not provided")]
        [SwaggerResponse(500, "Unexpected error")]
        [HttpGet]
        public IActionResult GetAvailableTransportsByProvidedWeight([FromQuery] double weight)
        {
            if (weight == 0)
            {
                return new BadRequestObjectResult(new { status = "Weight greater than 0 not provided", content = (string)null });
            }

            var query = from transport in context.Transports
                        join transportType in context.TransportTypes on transport.TransportTypeId equals transportType.Id
                        where transport.MinimalWeight <= weight && weight <= transport.MaximalWeight
                        select new AvailableTransportDto
                        {
                            Id = transport.Id,
                            Price = transport.Price,
                            TransportType = transport.TransportType.Name
                        };

            var transports = query.ToList();

            return new OkObjectResult(new { status = "OK", content = transports });
        }

        [SwaggerOperation(summary: "Create new transport", description: "Endpoint used for creation of new transport. Transport is sent as body")]
        [SwaggerResponse(201, "Successfully created")]
        [SwaggerResponse(400, "Bad transport type id sent")]
        [SwaggerResponse(415, "Bad request body sent")]
        [SwaggerResponse(500, "Unexpected error")]
        [HttpPost]
        public IActionResult CreateNewTransport([FromBody] TransportBodyDto bodyTransport)
        {
            var transportType = context.TransportTypes.FirstOrDefault(transportType => transportType.Id == bodyTransport.TransportTypeId);
            if (transportType == null)
            {
                return new BadRequestObjectResult(new { status = "Transport type sent in body doesn't exist", content = (string)null });
            }

            var allTransports = context.Transports.Where(t => t.TransportTypeId == transportType.Id).ToList();
            if (allTransports.FirstOrDefault(t => t.MinimalWeight <= bodyTransport.MinimalWeight && bodyTransport.MinimalWeight <= t.MaximalWeight) is not null)
            {
                return new BadRequestObjectResult(new { status = "Minimal weight you are trying to set is part of existing range", content = (string)null });
            }

            if (allTransports.FirstOrDefault(t => t.MinimalWeight <= bodyTransport.MaximalWeight && bodyTransport.MaximalWeight <= t.MaximalWeight) is not null)
            {
                return new BadRequestObjectResult(new { status = "Maximal weight you are trying to set is part of existing range", content = (string)null });
            }

            var newTransport = new Transport()
            {
                MaximalWeight = bodyTransport.MaximalWeight,
                MinimalWeight = bodyTransport.MinimalWeight,
                Price = bodyTransport.Price,
                TransportTypeId = transportType.Id,
                TransportType = transportType
            };

            try
            {
                context.Transports.Add(newTransport);
                context.SaveChangesAsync();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[CreateNewTransport]Successfully created transport with price " + bodyTransport.Price, null);

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[CreateNewTransport]Creating new transport not successful", ex);
                return new BadRequestObjectResult(new { status = "Saving in database not successful", content = (string)null });
            }

            return new StatusCodeResult(201);
        }

        [SwaggerOperation(summary: "Updates existing transport", description: "Endpoint used for updating of existing transport. There is applied advanced algorithm for coordianation of weight ranges")]
        [SwaggerResponse(200, "Successfully updated")]
        [SwaggerResponse(400, "Bad transport id provided")]
        [SwaggerResponse(415, "Bad request body sent")]
        [SwaggerResponse(500, "Unexpected error")]
        [HttpPut]
        public IActionResult UpdateTransportDetails([FromQuery] int transportId, [FromBody] TransportBodyDto newTrasport)
        {
            var transportType = context.TransportTypes.FirstOrDefault(transportType => transportType.Id == newTrasport.TransportTypeId);
            if (transportType == null)
            {
                return new BadRequestObjectResult(new { status = "Transport type sent in body doesn't exist", content = (string)null });
            }

            var allTransports = context.Transports.OrderBy(t => t.MinimalWeight).Where(t => t.TransportTypeId == transportType.Id).ToList();
            Transport transportForUpdate = null;
            var availableTransportsExcludingTransportForUpdate = new List<Transport>();
            foreach (var transport in allTransports)
            {
                if (transport.Id == transportId)
                {
                    transportForUpdate = transport;
                } else
                {
                    availableTransportsExcludingTransportForUpdate.Add(transport);
                }
            }
            
            if (transportForUpdate == null)
            {
                return new BadRequestObjectResult(new { status = "Bad transport id provided", content = (string)null });
            }

            var previousTransport = availableTransportsExcludingTransportForUpdate.FirstOrDefault(t => t.MaximalWeight == transportForUpdate.MinimalWeight - 1);
            if (previousTransport is not null)
            {
                if (previousTransport.MinimalWeight <= newTrasport.MinimalWeight && newTrasport.MinimalWeight <= previousTransport.MaximalWeight || previousTransport.MaximalWeight <= newTrasport.MinimalWeight)
                {
                    previousTransport.MaximalWeight = newTrasport.MinimalWeight - 1;
                }
                else
                {
                    return new BadRequestObjectResult(new { status = "Updating through multiple ranges not possible", content = (string)null });
                }
            } else
            {
                newTrasport.MinimalWeight = 0;
            }

            var nextTransport = availableTransportsExcludingTransportForUpdate.FirstOrDefault(t => t.MinimalWeight == transportForUpdate.MaximalWeight + 1);
            if (nextTransport is not null)
            {
                if (nextTransport.MinimalWeight <= newTrasport.MaximalWeight && newTrasport.MaximalWeight <= nextTransport.MaximalWeight || newTrasport.MaximalWeight <= nextTransport.MinimalWeight)
                {
                    nextTransport.MinimalWeight = newTrasport.MaximalWeight + 1;
                } else
                {
                    if (newTrasport.MinimalWeight != 0 || newTrasport.MaximalWeight > nextTransport.MaximalWeight)
                        return new BadRequestObjectResult(new { status = "Updating through multiple ranges not possible", content = (string)null });
                }
            }


            transportForUpdate.Price = newTrasport.Price;
            transportForUpdate.MinimalWeight = newTrasport.MinimalWeight;
            transportForUpdate.MaximalWeight = newTrasport.MaximalWeight;
            transportForUpdate.TransportType = transportType;
            transportForUpdate.TransportTypeId = transportType.Id;

            try
            {
                if( previousTransport is not null)
                {
                    context.Update(previousTransport);
                }
                if (nextTransport is not null)
                {
                    context.Update(nextTransport);
                }
                context.Update(transportForUpdate);
                context.SaveChangesAsync();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[UpdateTransportDetails]Successfully updated transport with id " + newTrasport.Id, null);
            }
            catch(Exception ex)
            {
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[UpdateTransportDetails] Updating transport with id " + newTrasport.Id + " not successful", ex);
                return new BadRequestObjectResult(new { status = "Saving in database not successful", content = (string)null });
            }

            return new OkObjectResult(new { status = "Successfully updated", content = (string)null });
        }

        [SwaggerOperation(summary: "Deletes existing transport", description: "Endpoint used for deletion of existing transport. There is applied rule that only transport record with weight range which has the biggest values can be deleted")]
        [SwaggerResponse(200, "Successfully deleted")]
        [SwaggerResponse(400, "Bad transport id provided")]
        [SwaggerResponse(500, "Unexpected error")]
        [HttpDelete]
        public IActionResult DeleteTransport([FromQuery] int transportId)
        {
            var allTransports = context.Transports.ToList();
            var transport = allTransports.FirstOrDefault(transport => transport.Id == transportId);
            if (transport is null)
            {
                return new BadRequestObjectResult(new { status = "Bad transport id provided", content = (string)null });
            }
            var transportsWithParticularType = allTransports.Where(t => t.TransportTypeId == transport.TransportTypeId);

            var nextTransport = transportsWithParticularType.FirstOrDefault(t => t.MinimalWeight == transport.MaximalWeight + 1);
            if (nextTransport is not null)
            {
                return new BadRequestObjectResult(new { status = "Deleting of the range with the biggest values is only allowed", content = (string)null });
            }

            try
            {
                context.Transports.Remove(transport);
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[DeleteTransport]Successfully deleted transport with id " + transport.Id, null);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[DeleteTransport] Deletion for transport with id " + transport.Id + " not successful", ex);
                return new BadRequestObjectResult(new { status = "Saving in dabase not successful", content = (string)null });
            }

            return new OkObjectResult(new { status = "Transport successfully deleted", content = (string)null });
        }
    }
}
