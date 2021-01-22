using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Controllers
{
    [ApiController]
    [Route("api/transport")]
    public class TransportController
    {
        [HttpGet("all")]
        public IActionResult GetAllTransportTypes()
        {
            return new OkObjectResult(new { status = "OK", content = (string)null });
        }
    }
}
