using loggerMicroservice.Data;
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
        public ILogRepository LogRepository { get; }

        public LoggerController(ILogRepository logRepository)
        {
            LogRepository = logRepository;
        }
        [HttpPost]
        public IActionResult Log([FromForm]Log log)
        {
                Log insertedLog = LogRepository.InsertLog(log);
                return Ok(insertedLog);
        }
        [HttpGet("{microservice}")]
        public ActionResult<List<Log>> GetLogsByService(string microservice)
        {
            return Ok(LogRepository.GetLogsByMicroservice(microservice));
        }
        [HttpGet("/interval")]
        public ActionResult<List<Log>> GetLogsByInterval(DateTime from ,DateTime to)
        {
            return Ok(LogRepository.GetLogsByInterval(from, to));
        }
        [HttpGet("/level/{logLevel}")]
        public ActionResult<List<Log>> GetLogsByLevel(string logLevel)
        {
            return Ok(LogRepository.GetLogsByLogLevel(logLevel));
        }
    }
}
