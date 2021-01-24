using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transport_Service.Data;
using Transport_Service.Models.DTOs;
using Transport_Service.Models.Entities;

namespace Transport_Service.Controllers
{
    [ApiController]
    [Route("api/transport")]
    public class TransportController
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor contextAccessor;

        public TransportController(ApplicationDbContext context, ILogger logger, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
        }

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

        [HttpPost]
        public IActionResult CreateNewTransport([FromBody] TransportBodyDto bodyTransportType)
        {
            var transportType = context.TransportTypes.FirstOrDefault(transportType => transportType.Id == bodyTransportType.TransportTypeId);
            if (transportType == null)
            {
                return new BadRequestObjectResult(new { status = "Transport type sent in body doesn't exist", content = (string)null });
            }

            var allTransports = context.Transports.Where(t => t.TransportTypeId == transportType.Id).ToList();
            if (allTransports.FirstOrDefault(t => t.MinimalWeight <= bodyTransportType.MinimalWeight && bodyTransportType.MinimalWeight <= t.MaximalWeight) is not null)
            {
                return new BadRequestObjectResult(new { status = "Minimal weight you are trying to set is part of existing range", content = (string)null });
            }

            if (allTransports.FirstOrDefault(t => t.MinimalWeight <= bodyTransportType.MaximalWeight && bodyTransportType.MaximalWeight <= t.MaximalWeight) is not null)
            {
                return new BadRequestObjectResult(new { status = "Maximal weight you are trying to set is part of existing range", content = (string)null });
            }

            var newTransport = new Transport()
            {
                MaximalWeight = bodyTransportType.MaximalWeight,
                MinimalWeight = bodyTransportType.MinimalWeight,
                Price = bodyTransportType.Price,
                TransportTypeId = transportType.Id,
                TransportType = transportType
            };

            try
            {
                //TODO: Logger
                context.Transports.Add(newTransport);
                context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                //TODO: Logger
                return new BadRequestObjectResult(new { status = "Saving in database not successful", content = (string)null });
            }

            return new StatusCodeResult(201);
        }

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
            }
            catch
            {
                return new BadRequestObjectResult(new { status = "Saving in database not successful", content = (string)null });
            }

            return new OkObjectResult(new { status = "Successfully updated", content = (string)null });
        }

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
                //TODO: Logger
            }
            catch (Exception ex)
            {
                //TODO: Logger
                return new BadRequestObjectResult(new { status = "Saving in dabase not successful", content = (string)null });
            }

            return new OkObjectResult(new { status = "Transport successfully deleted", content = (string)null });
        }
    }
}
