using loggerMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace loggerMicroservice.Controllers
{
    [ApiController]
    [Route("api/log")]
    public class LoggerController:ControllerBase
    {
        [HttpPost]
        public IActionResult Log([FromForm]Log log)
        {
            Console.WriteLine(log);
            return Ok();
        }
    }
}
