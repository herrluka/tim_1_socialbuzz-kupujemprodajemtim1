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
        public IActionResult GetAvailableTransportsByProvidedPrice([FromQuery] double price)
        {
            if (price == 0)
            {
                return new BadRequestObjectResult(new { status = "Price greater than 0 not provided", content = (string)null });
            }

            var query = from transport in context.Transports
                        join transportType in context.TransportTypes on transport.TransportTypeId equals transportType.Id
                        where transport.MinimalWeight < price && price < transport.MaximalWeight
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

        [HttpDelete]
        public IActionResult DeleteTransport([FromQuery] int transportId)
        {
            var transport = context.Transports.FirstOrDefault(type => type.Id == transportId);
            if (transport is null)
            {
                return new BadRequestObjectResult(new { status = "Bad transport id provided", content = (string)null });
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

            return new OkObjectResult(new { status = "Saving in dabase not successful", content = (string)null });
        }

        [HttpPut]
        public IActionResult UpdateTransportDetails([FromQuery] int transportId, [FromBody] TransportBodyDto newTrasport)
        {
            var transport = context.Transports.FirstOrDefault(transport => transport.Id == transportId);
            if (transport == null)
            {
                return new BadRequestObjectResult(new { status = "Bad transport id provided", content = (string)null });
            }

            var transportType = context.TransportTypes.FirstOrDefault(transportType => transportType.Id == newTrasport.TransportTypeId);
            if (transportType == null)
            {
                return new BadRequestObjectResult(new { status = "Transport type sent in body doesn't exist", content = (string)null });
            }

            transport.Price = newTrasport.Price;
            transport.MinimalWeight = newTrasport.MinimalWeight;
            transport.MaximalWeight = newTrasport.MaximalWeight;
            transport.TransportType = transportType;
            transport.TransportTypeId = transportType.Id;

            try
            {
                //TODO : Logger
                context.SaveChangesAsync();
            }
            catch
            {
                return new BadRequestObjectResult(new { status = "Saving in database not successful", content = (string)null });
            }

            return new StatusCodeResult(201);
        }
    }
}
