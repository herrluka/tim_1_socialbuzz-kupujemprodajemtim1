using AutoMapper;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactionsService.Data;
using ReactionsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace ReactionsService.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ReactionsController : ControllerBase
    {
        private readonly IReactionRepository reactionRepository;
        private readonly IProductMockRepository productMockRepository;
        private readonly IMapper mapper;
        private readonly Logger logger;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ITypeOfReactionRepository typeOfReactionRepository;


        public ReactionsController(ITypeOfReactionRepository typeOfReactionRepository, IHttpContextAccessor contextAccessor, IMapper mapper, IReactionRepository reactionRepository, IProductMockRepository productMockRepository, Logger logger)
        {
            this.reactionRepository = reactionRepository;
            this.productMockRepository = productMockRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
            this.typeOfReactionRepository = typeOfReactionRepository;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userID}")]
        public ActionResult<List<ReactionsDto>> GetAllReactions(int userID)
        {
            var reactions = reactionRepository.GetReactions(userID);

            if (reactions == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no reactions.");
            }

            return Ok(mapper.Map<List<ReactionsDto>>(reactions));

        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("byProduct/{productID}/{userID}")]
        public ActionResult GetReactionsByProductID( int productID, int userID)
        {
            if(productMockRepository.GetProductByID(productID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no product with given ID");
            }

            var reactions = reactionRepository.GetRectionByProductID(productID, userID);

            if(reactions.Count == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "This product has no reactions added");
            }

            return Ok(mapper.Map<List<ReactionsDto>>(reactions));
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("{userID}")]
        public IActionResult AddReactionToProduct([FromBody] ReactionCreateDto reaction, int userID)
        {
            if(productMockRepository.GetProductByID(reaction.ProductID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Product with given ID does not exist");
            }

            if(typeOfReactionRepository.GetTypeOfReactionByID(reaction.TypeOfReactionID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Type of reaction with given ID does not exist!");

            }

            Reactions reactionEntity = mapper.Map<Reactions>(reaction);
            reactionEntity.UserID = userID;

            try
            {  
                reactionRepository.AddReaction(reactionEntity);
                reactionRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new reaction with ID {0} in database", reactionEntity.ReactionID), null);

                return StatusCode(StatusCodes.Status201Created, "Reaction is successfully created!");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Reaction with ID {0} not created, message: {1}", reactionEntity.ReactionID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, "Create error");
            }
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public IActionResult UpdateReaction([FromBody] ReactionUpdateDto reaction)
        {
            if(reactionRepository.GetReactionByID(reaction.ReactionID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no reaction with given ID!");
            }

            if (typeOfReactionRepository.GetTypeOfReactionByID(reaction.TypeOfReactionID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Type of reaction with given ID does not exist!");

            }
            try
            {
                Reactions newReaction = mapper.Map<Reactions>(reaction);

                Reactions oldReaction = reactionRepository.GetReactionByID(reaction.ReactionID);

                if (newReaction.ProductID != oldReaction.ProductID)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Product ID can not be changed!");

                }
               newReaction.UserID = oldReaction.UserID;

                mapper.Map(newReaction, oldReaction);

                reactionRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated reaction with ID {0} in database", reaction.ReactionID), null);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while updating reaction with ID {0}, message: {1}", reaction.ReactionID, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");
            }
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{reactionID}")]
        public IActionResult DeleteReacion(Guid reactionID)
        {
            var reaction = reactionRepository.GetReactionByID(reactionID);
            if(reaction == null)
            {
                return NotFound();
            }
            try
            {
                reactionRepository.DeleteReaction(reactionID);
                reactionRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted reaction with ID {0} from database", reactionID), null);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting reaction with ID {0}, message: {1}", reaction.ReactionID, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }
    }
}
