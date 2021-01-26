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
using WebApplication1.Models;

namespace ReactionsService.Controllers
{
    [ApiController]
    [Route("[controller]")]


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

        [HttpGet]
         public ActionResult<List<TypeOfReactionDto>> GetAllTypesOfReaction(int userID)
         {
            var types = typeOfReactionRepository.GetAllTypesOfReaction();

            if(types == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no types of reaction.");
            }

            return Ok(mapper.Map<List<TypeOfReactionDto>>(types));
         }


        [HttpGet("{typeOfReactionID}")]
        public ActionResult GetTypeOfReactionByID (int typeOfReactionID)
        {
            var type = typeOfReactionRepository.GetTypeOfReactionByID(typeOfReactionID);

            if(type == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no type of reaction with given ID");
            }

            return Ok(mapper.Map<TypeOfReactionDto>(type));

        }

        [HttpPost]
        public IActionResult AddTypeOfReaction([FromBody] TypeOfReactionCreateDto typeOfReaction)
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

        [HttpPut]
        public ActionResult UpdateTypeOfReaction(TypeOfReactionUpdateDto typeOfReaction)
        {
            var oldType = typeOfReactionRepository.GetTypeOfReactionByID(typeOfReaction.TypeOfReactionID);

            if (oldType == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no type of reaction with given ID");
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


        [HttpDelete("{typeOfReactionID}")]
        public IActionResult DeleteTypeOfReaction(int typeOfReactionID)
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

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting reaction with ID {0}, message: {1}", typeOfReactionID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
