using AutoMapper;
using CommentingService.Data;
using CommentingService.Data.Comment;
using CommentingService.Model;
using CommentingService.Model.CreateDTO;
using CommentingService.Model.UpdateDTO;
using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class CommentsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly ICommentRepository commentRepository;
        private readonly IProductMockRepository productRepository;
        private readonly Logger logger;

        public CommentsController(Logger logger, IProductMockRepository productRepository, IHttpContextAccessor contextAccessor, IMapper mapper, ICommentRepository commentRepository)
        {
            this.mapper = mapper;
            this.contextAccessor = contextAccessor;
            this.commentRepository = commentRepository;
            this.productRepository = productRepository;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult GetAllComments()
        {
           
            var comments = commentRepository.GetAllComments();
            if (comments == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no comments.");
            }

            return Ok(comments);
        }


        /// <summary>
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("byProductID/{productID}/{userID}")]
        public ActionResult <List<Comments>> GetCommentsByProductID(int productID, int userID)
        {

            var sellerID = productRepository.GetProductByID(productID).SellerID;

            /// korisnik ne moze videti komentare proizvoda cije vlasnike je on blokirao ili su njega blokirali
            if (commentRepository.CheckDidIBlockedSeller(userID, sellerID) == true)
            {
                return StatusCode(StatusCodes.Status400BadRequest, String.Format("You can not see comments on product of seller with id {0} ", sellerID));
            }

            if (productRepository.GetProductByID(productID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Product with given ID does not exist");
            }

            /// korisnik ne moze videti komentare koji su dodali korisnici koje je on blokirao ili koji su njega blokirali
            var comments = commentRepository.GetCommentsByProductID(productID, userID);

            if (comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "This product has no comments added");
            }

            return Ok(comments);
        }


        [HttpPost("{userID}")]
        public IActionResult AddComment(CommentCreateDto commentDto, int userID)
        {
            if(productRepository.GetProductByID(commentDto.ProductID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Product with given ID does not exist");
            }

            var comment = mapper.Map<Comments>(commentDto);
            var product = productRepository.GetProductByID(comment.ProductID);
            var sellerID = product.SellerID;

            if (commentRepository.CheckDoIFollowSeller(userID, sellerID) == false)
            {
                return StatusCode(StatusCodes.Status400BadRequest, String.Format("You do not follow user with id {0} and you can not comment his products", sellerID));
            }

            comment.UserID = userID;
            comment.CommentDate = DateTime.Today;

            try
            {
                commentRepository.AddComment(comment);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new comment with ID {0} in database", comment.CommentID), null);

                return StatusCode(StatusCodes.Status201Created, "Reaction is successfully created!");
            }
            catch (Exception ex)
            {
               logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Comment with ID {0} not created, message: {1}", comment.CommentID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, "Create error " + ex.Message);
            }


        }

        [HttpPut]
        public IActionResult UpdateComment(CommentUpdateDto comment)
        {
            if (commentRepository.GetCommentByID(comment.CommentID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no comment with given ID!");
            }

            var oldComment = commentRepository.GetCommentByID(comment.CommentID);
            var newComment = mapper.Map<Comments>(comment);

            if(oldComment.ProductID != newComment.ProductID)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Product ID can not be changed!");
            }

            try
            {
                newComment.UserID = oldComment.UserID;
                newComment.CommentDate = oldComment.CommentDate;

                mapper.Map(newComment, oldComment);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated comment with ID {0} in database", comment.CommentID), null);

                return StatusCode(StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Comment with ID {0} not updated, message: {1}", comment.CommentID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, "Update error");


            }

        }

        [HttpDelete("{commentID}")]
        public IActionResult DeleteComment(Guid commentID)
        {
            var comment = commentRepository.GetCommentByID(commentID);
            if (comment == null)
            {
                return NotFound();
            }
            try
            {
                commentRepository.DeleteComment(commentID);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted comment with ID {0} from database", commentID), null);

                return NoContent();
            }
            catch (Exception ex)
            {
               logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Error while deleting comment with ID {0}, message: {1}", commentID, ex.Message), null);
                return StatusCode(StatusCodes.Status500InternalServerError, "Delete error");
            }
        
    }
       

    }
}
