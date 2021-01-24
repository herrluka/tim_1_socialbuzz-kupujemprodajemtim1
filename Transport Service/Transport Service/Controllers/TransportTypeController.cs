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
    public class TransportTypeController
    {
        private readonly ApplicationDbContext context;
        private readonly Logger logger;
        private readonly IHttpContextAccessor contextAccessor;

        public TransportTypeController(ApplicationDbContext context, Logger logger, IHttpContextAccessor contextAccessor)
        {
            this.context = context;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
        }

        [HttpGet("type/all")]
        public IActionResult GetAllTransportTypes()
        {
            var transportTypes = context.TransportTypes.ToList();
            return new OkObjectResult(new { status = "OK", content = transportTypes });
        }

        [HttpGet("type/{id}")]
        public IActionResult GetAllTransportTypes([FromRoute] int id)
        {
            var transportType = context.TransportTypes.FirstOrDefault(t => t.Id == id);
            return new OkObjectResult(new { status = "OK", content = new TransportTypeDto
            {
                Name = transportType.Name
            }
            });
        }

        [HttpPost("type")]
        public IActionResult CreateNewTransportType([FromBody] TransportTypeDto bodyTransportType)
        {

            var newTransportType = new TransportType()
            {
                Name = bodyTransportType.Name
            };

            try
            {
                context.TransportTypes.Add(newTransportType);
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[CreateNewTransportType]Successfully created transport type with name " + bodyTransportType.Name, null);

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", "[CreateNewTransportType]Error occured for transport type " + bodyTransportType.Name + " creation", ex);
                return new BadRequestObjectResult(new { status = "Saving in database not successful", content = (string)null });
            }

            return new StatusCodeResult(201);
        }

        [HttpDelete("type")]
        public IActionResult DeleteTransportType([FromQuery] int transportTypeId)
        {
            var transportType = context.TransportTypes.FirstOrDefault(type => type.Id == transportTypeId);
            if (transportType is null)
            {
                return new BadRequestObjectResult(new { status = "Bad transport id provided", content = (string)null });
            }

            try
            {
                context.TransportTypes.Remove(transportType);
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", "[DeleteTransportType]Successfully deleted transport type with name" + transportType.Name, null);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", "[DeleteTransportType]Saving in database not successful for type " + transportType.Name, null);
                return new BadRequestObjectResult(new { status = "Saving in dabase not successful", content = (string)null });
            }

            return new OkObjectResult(new { status = "Saving in dabase not successful", content = (string)null });
        }

    }
}
