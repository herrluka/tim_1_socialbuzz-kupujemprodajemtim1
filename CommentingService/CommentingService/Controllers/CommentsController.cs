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
    /// <summary>
    /// Kontroler koji izvrsava CRUD operacije nad tabelom Comment
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]

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

        /// <summary>
        /// Vraća kreirane komentare    
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva
        /// GET 'https://localhost:44328/comments' \
        ///     --header 'CommunicationKey: Super super tezak kljuc'
        /// </remarks>
        /// <response code="200">Vraća listu komentara</response>
        /// <response code="404">Nisu pronađeni komenari</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult GetAllComments([FromHeader(Name = "CommunicationKey")] string key)
        {
           
            var comments = commentRepository.GetAllComments();
            if (comments == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no comments.");
            }

            return Ok(comments);
        }


        /// <summary>
        /// Vraća komentare dodeljene specificiranom proizvodu 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET 'https://localhost:44328/comments/byProductID' \
        ///  Primer zahteva koji prolazi \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'productID = 1' \
        ///     --param  'userID = 4' \
        /// Primer zahteva koji ne prolazi jer je korisnik sa ID-jem 4 blokirao korisnika sa ID-jem 2, koji je vlasnik proizvoda sa ID-jem 2, i ne može videti njegove proizvode \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'productID = 2' \
        ///     --param  'userID = 4'
        /// </remarks>
        /// <param name="productID">ID proizvoda</param>
        /// <param name="userID">ID korisnika koji šalje zahtev</param>
        /// <response code="200">Vraća listu komentara za specificirani proizvod</response>
        /// <response code="400">Loše kreiran zahtev</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađen komenar sa zadatim ID-jem proizvoda</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byProductID")]
        public ActionResult <List<Comments>> GetCommentsByProductID([FromHeader(Name = "CommunicationKey")] string key, [FromQuery] int productID, [FromQuery] int userID)
        {

            if (productRepository.GetProductByID(productID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Product with given ID does not exist");
            }
            var sellerID = productRepository.GetProductByID(productID).SellerID;


            /// korisnik ne moze videti komentare proizvoda cije vlasnike je on blokirao ili su njega blokirali
            if (commentRepository.CheckDidIBlockedSeller(userID, sellerID) == true)
            {
                return StatusCode(StatusCodes.Status400BadRequest, String.Format("You can not see products with sellerID {0} ", sellerID));
            }

            /// korisnik ne moze videti komentare koji su dodali korisnici koje je on blokirao ili koji su njega blokirali
            var comments = commentRepository.GetCommentsByProductID(productID, userID);

            if (comments.Count == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "This product has no comments added");
            }

            return Ok(comments);
        }


        /// <summary>
        /// Kreira novi komenar
        /// </summary>
        /// <param name="commentDto">Model komenara koji se dodaje</param>
        /// <param name="userID">ID korisnika koji pokreće zahtev</param>
        /// <returns></returns>
        /// <remarks>
        /// POST 'https://localhost:44328/comments/' \
        /// Primer zahteva za kreiranje novog komentara koji prolazi \
        ///  --header 'CommunicationKey: Super super tezak kljuc' \
        ///  --param 'userID = 4' \
        /// {     \
        ///  "productID": 1, \
        ///  "content": "New comment" \
        /// } \
        ///  Primer zahteva za kreiranje novog komentara koji ne prolazi jer korisnik sa ID-jem 4 ne prati korisnika sa ID-jem 3, koji je vlasnik proizvoda sa ID-jem 3, i ne može komentarisati njegove proizvode \
        ///  --header 'CommunicationKey: Super super tezak kljuc' \
        ///  --param 'userID = 4' \
        /// {     \
        ///  "productID": 3, \
        ///  "content": "New comment" \
        /// }
        /// </remarks>
        /// <response code="201">Vraća potvrdu da je kreiran novi komentar</response>
        /// <response code="400">Loše kreiran zahtev</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="500">Greška na serveru prilikom čuvanja komentara.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]
        public IActionResult AddComment([FromHeader(Name = "CommunicationKey")] string key, CommentCreateDto commentDto, [FromQuery] int userID)
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
                return StatusCode(StatusCodes.Status400BadRequest, String.Format("You are not following user with id {0} and you can not comment his products", sellerID));
            }

            comment.UserID = userID;
            comment.CommentDate = DateTime.Today;

            try
            {
                commentRepository.AddComment(comment);
                commentRepository.SaveChanges();
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created new comment with ID {0} in database", comment.CommentID), null);

                return StatusCode(StatusCodes.Status201Created, "Comment is successfully created!");
            }
            catch (Exception ex)
            {
               logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Comment with ID {0} not created, message: {1}", comment.CommentID, ex.Message), null);

                return StatusCode(StatusCodes.Status500InternalServerError, "Create error " + ex.Message);
            }


        }

        /// <summary>
        /// Vrši izmenu jednog komentara na osnovu ID-ja komentara
        /// </summary>
        /// <param name="comment">Model komentara koji se ažurira</param>
        /// <returns></returns>
        /// <remarks>
        /// PUT 'https://localhost:44328/comments' \
        /// Primer zahteva za modifikaciju komentara    \
        ///  --header 'CommunicationKey: Super super tezak kljuc'  \
        ///{ \
        /// "commentID": "23209e86-e2a5-4691-d1e7-48d8c11a2ff5", \
        /// "productID": 3, \
        ///  "content": "Updated!" \
        ///  } 
        /// </remarks>
        /// <response code="200">Vraća potvrdu da je uspešno izmenjen komentar</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađen komentar za ažuriranje</response>
        /// <response code="500">Greška na serveru prilikom modifikacije komentara</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut]
        public IActionResult UpdateComment([FromHeader(Name = "CommunicationKey")] string key, [FromBody] CommentUpdateDto comment)
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

        /// <summary>
        /// Vrši brisanje jednog komentara na osnovu ID-ja komentara
        /// </summary>
        /// <param name="commentID">ID komentara koji se briše</param>
        /// <remarks>        
        /// Primer zahteva
        /// DELETE 'https://localhost:44328/comments' \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'commentID = 734E8DC9-5D60-45A8-6739-08D8C3A8AEC3'
        /// </remarks>
        /// <response code="204">Komentar je uspešno obrisan</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađen komentar za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja komentara</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public IActionResult DeleteComment([FromHeader(Name = "CommunicationKey")] string key, [FromQuery] Guid commentID)
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
