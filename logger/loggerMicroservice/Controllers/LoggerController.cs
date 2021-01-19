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
        public IActionResult InsertLog([FromForm]Log log)
        {
                Log insertedLog = LogRepository.InsertLog(log);
                return Ok(insertedLog);
        }
        [HttpGet]
        public ActionResult<List<Log>> GetLogs(string microservice,string logLevel,DateTime from,DateTime to)
        {

           var collection= LogRepository.GetLogs(microservice, logLevel, from, to);
            Console.WriteLine(collection);
            return Ok(collection);
            
        }
    }
}
