using AutoMapper;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactionsService.Data;
using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Controllers
{
    /// <summary>
    /// Kontroler koji izvesava CRUD operacij nad tabelom Tip reakcije
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]


    public class TypeOfReactionController : ControllerBase
    {
        private readonly ITypeOfReactionRepository typeOfReactionRepository;
        private readonly IMapper mapper;
        private readonly Logger logger;
        private readonly IHttpContextAccessor contextAccessor;


        public TypeOfReactionController(ITypeOfReactionRepository typeOfReactionRepository, IMapper mapper, Logger logger, IHttpContextAccessor contextAccessor)
        {
            this.typeOfReactionRepository = typeOfReactionRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Vraca kreirane tipove reakcija
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva
        /// GET 'http://localhost:44360/typeOfReaction/' \
        ///     --header 'CommunicationKey: Super super tezak kljuc' 
        /// </remarks>
        /// <response code="200">Vraća listu tipova reakcija</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nisu pronađeni tipovi reakcija</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
         public ActionResult<List<TypeOfReactionDto>> GetAllTypesOfReaction([FromHeader(Name = "CommunicationKey")] string key)
         {
            var types = typeOfReactionRepository.GetAllTypesOfReaction();

            if(types == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no types of reaction.");
            }

            return Ok(mapper.Map<List<TypeOfReactionDto>>(types));
         }


        /// <summary>
        /// Vraća tip reakcije na osnovu ID-ja tipa reakcija
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva
        /// GET 'https://localhost:44360/typeOfReaction/byID' \
        ///     --header 'CommunicationKey: Super super tezak kljuc'  \
        ///     --param  'typeOfReactionID = 1'  
        /// </remarks>
        /// <param name="typeOfReactionID">ID tipa reakacije</param>
        /// <response code="200">Vraća tip rekacije</response>
        /// <response code="404">Nije pronađen tip reakcije sad prosleđenim ID-jem tipa reakcije</response>
        /// <response code="401">Pogrešna autentifikacija</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byID")]
        public ActionResult GetTypeOfReactionByID ([FromHeader(Name = "CommunicationKey")] string key, [FromQuery] int typeOfReactionID)
        {
            var type = typeOfReactionRepository.GetTypeOfReactionByID(typeOfReactionID);

            if(type == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no type of reaction with given ID");
            }

            return Ok(mapper.Map<TypeOfReactionDto>(type));

        }

        /// <summary>
        /// Kreira novi tip reakcije
        /// </summary>
        /// <param name="typeOfReaction">Model tipa reakcije</param>
        /// <param name="key"> CommunicationKey</param>
        /// <remarks>
        /// Primer zahteva za kreiranje novog tipa reakcije \
        ///   --header 'CommunicationKey: Super super tezak kljuc' \
        /// POST https://localhost:44360/typeOfReaction \
        /// {   \
        ///  "reactionName": "Insert", \
        ///  "url": "https://upload.wikimedia.org/wikipedia/commons/8/85/Smiley.svg" \
        /// }
        /// </remarks>
        /// <response code="201">Vraća potvrdu da je uspešno kreiran novi tip reakcije</response>
        /// <response code="401">Pogrešna autentifikacija</response>
        /// <response code="500">Došlo je do greške na serveru prilikom kreiranja tipa reakcije</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]
        public IActionResult AddTypeOfReaction([FromHeader(Name = "CommunicationKey")] string key, [FromBody] TypeOfReactionCreateDto typeOfReaction)
        {
            TypeOfReaction type = mapper.Map<TypeOfReaction>(typeOfReaction);

            try
            {
                typeOfReactionRepository.AddTypeOfReaction(type);
                typeOfReactionRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new type of reaction with ID {0} in database", type.TypeOfReactionID), null);
                return StatusCode(StatusCodes.Status201Created, "Type of reaction is successfully created!");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Reaction with ID {0} not created, message: {1}", type.TypeOfReactionID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, "Create error");
            }

        }

        /// <summary>
        /// Vrši izmenu jednog tipa rekacije na osnovu ID-ja tipa reakcije
        /// </summary>
        /// <param name="typeOfReaction">Model tipa reakcije koji se ažurira</param>
        /// <remarks>
        /// Primer zahteva za modifikovanje tipa reakcije \
        ///    --header 'CommunicationKey: Super super tezak kljuc' \
        /// PUT  https://localhost:44360/typeOfReaction \
        /// { \
        /// "typeOfReactionID": 8, \
        /// "reactionName": "Big heart", \
        /// "url": "https://upload.wikimedia.org/wikipedia/commons/8/85/Smiley.svg" \
        ///}
        /// </remarks>
        /// <response code="200">Vraća potvrdu da je uspešno ažuriran tip reakcije</response>
        /// <response code="404">Tip reakcije koji se ažurira nije pronađen</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="500">Došlo je do greške na serveru prilikom ažuriranja tipa reakcije</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut]
        public IActionResult UpdateTypeOfReaction([FromHeader(Name = "CommunicationKey")] string key, TypeOfReactionUpdateDto typeOfReaction)
        {
            var oldType = typeOfReactionRepository.GetTypeOfReactionByID(typeOfReaction.TypeOfReactionID);

            if (oldType == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no type of reaction with given ID");
            }
            
            var newType = mapper.Map<TypeOfReaction>(typeOfReaction);


            try
            {
                mapper.Map(newType, oldType);
                typeOfReactionRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated type of reaction with ID {0} in database", typeOfReaction.TypeOfReactionID), null);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating reaction with ID {0}, message: {1}", oldType.TypeOfReactionID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }

        /// <summary>
        /// Vrši brisanje jednog tipa reakcije na osnovu ID-ja tipa reakcije.
        /// </summary>
        /// <param name="typeOfReactionID">ID tipa reakcije koji se briše</param>
        /// <remarks>        
        /// Primer zahteva
        /// DELETE 'https://localhost:44360/typeOfReaction/' \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'typeOfReactionID = 14'
        /// </remarks>
        /// <response code="204">Tip reakcije je uspešno obrisan</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađen tip reakcije za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja tipa reakcije</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public IActionResult DeleteTypeOfReaction([FromHeader(Name = "CommunicationKey")] string key, [FromQuery] int typeOfReactionID)
        {
            var type = typeOfReactionRepository.GetTypeOfReactionByID(typeOfReactionID);

            if (type == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no type of reaction with given ID");
            }

            try
            {
                typeOfReactionRepository.DeleteTypeOfReaction(typeOfReactionID);
                typeOfReactionRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted type of reaction with ID {0} from database", typeOfReactionID), null);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting reaction with ID {0}, message: {1}", typeOfReactionID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
