using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
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

        public ReactionsController(IMapper mapper, IReactionRepository reactionRepository, IProductMockRepository productMockRepository)
        {
            this.reactionRepository = reactionRepository;
            this.productMockRepository = productMockRepository;
            this.mapper = mapper;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<List<ReactionsDto>> GetAllReactions()
        {
            var reactions = reactionRepository.GetReactions();

            if (reactions == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no reactions.");
            }

            return Ok(mapper.Map<List<ReactionsDto>>(reactions));

        }


    
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("byProduct/{productID}")]
        public ActionResult GetReactionsByProductID(int productID)
        {
            if(productMockRepository.GetProductByID(productID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no product with given ID");
            }

            var reactions = reactionRepository.GetRectionByProductID(productID);

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
                return StatusCode(StatusCodes.Status400BadRequest, "There is no product with given ID");
            }

            try
            {
                Reactions reactionEntity = mapper.Map<Reactions>(reaction);
                reactionEntity.UserID = userID;
                reactionRepository.AddReaction(reactionEntity);
                reactionRepository.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, "Reaction is successfully created!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create error");
            }

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut]
        public IActionResult UpdateReaction([FromBody] ReactionUpdateDto reaction)
        {
            try
            {
                Reactions newReaction = mapper.Map<Reactions>(reaction);

                Reactions oldReaction = reactionRepository.GetReactionByID(reaction.ReactionID);

                if (newReaction.ProductID != oldReaction.ProductID || newReaction.ReactionID != oldReaction.ReactionID)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Product ID or Reaction ID can not be changed!");

                }
               newReaction.UserID = oldReaction.UserID;

                mapper.Map(newReaction, oldReaction);

                reactionRepository.SaveChanges();

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception)
            {
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
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        }


    }
}
