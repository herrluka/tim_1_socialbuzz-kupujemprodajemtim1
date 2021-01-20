using loggerMicroservice.Data;
using loggerMicroservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
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

        /// <summary>
        /// Unos logova u bazu
        /// </summary>
        /// <param name="log">Log za unos</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer unosa novog loga \
        /// POST 'https://localhost:5003/api/log/' \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --header 'Content-Type: application/x-www-form-urlencoded' \
        ///     --data-urlencode 'LogLevel=Information' \
        ///     --data-urlencode 'EventID=' \
        ///     --data-urlencode 'RequestID=H52s0299503' \
        ///     --data-urlencode 'PreviousRequestID=No previous request ID' \
        ///     --data-urlencode 'Message=Testiram samo post za logove' \
        ///     --data-urlencode 'ExceptionType=' \
        ///     --data-urlencode 'ExceptionMessage=' \
        ///     --data-urlencode 'TimeOfAction=01/01/2021 5:28:29 PM' \
        ///     --data-urlencode 'Microservice=testingMicroservice'\
        /// </remarks>
        /// <response code="201">Vraca kreirani log</response>
        /// <response code="401">Greska pri autentifikaciji komunikacije izmedju klijenta i servisa</response>
        /// <response code="400">Lose uneta polja</response>
        /// <response code="500">Greska pri unosa vrednosti u bazu</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public IActionResult InsertLog([FromForm]Log log)
        {
            try
            {
                Log insertedLog = LogRepository.InsertLog(log);
                if (insertedLog == null)
                    throw new Exception("Something went wrong while inserting in the database!");
                return Created("", insertedLog);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Vraćanje logova na osnovu prosledjenih parametara
        /// </summary>
        /// <param name="microservice">Naziv mikroservisa koji je kreirao log</param>
        /// <param name="logLevel">Nivo logovanja (informacija, greška, upozorenje)</param>
        /// <param name="from">Datum od kojeg je log nastao</param>
        /// <param name="to">Datum do kojeg je log nastao</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer vracanja logova po datim parametrima
        /// GET 'https://localhost:5003/api/log?microservice=loggerMicroservice%26logLevel=Warning%26from=17-Jan-21%2011%3A41%3A10%20PM%26to=20-Jan-21%2011%3A41%3A10%20PM' \
        /// --header 'CommunicationKey: Super super tezak kljuc'
        /// </remarks>
        /// <response code="200">Vraca listu logova sa gore navedenim parametrima</response>
        /// <response code="401">Greska pri autentifikaciji komunikacije izmedju klijenta i servisa</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<List<Log>> GetLogs(string microservice,string logLevel,DateTime from,DateTime to)
        {

           var collection= LogRepository.GetLogs(microservice, logLevel, from, to);
            if (collection.Count == 0)
                return NoContent();
            return Ok(collection);
            
        }
    }
}
