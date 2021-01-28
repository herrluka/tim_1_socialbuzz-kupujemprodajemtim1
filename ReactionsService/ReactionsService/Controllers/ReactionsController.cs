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
using WebApplication1.Data;
using WebApplication1.Models;

namespace ReactionsService.Controllers
{
    /// <summary>
    /// Kontroler koji izvršava CRUD operacije nad Reactions tabelom
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]

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

        /// <summary>
        /// Vraća kreirane reakcije
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva
        /// GET 'http://localhost:44360/reactions/' \
        ///     --header 'CommunicationKey: Super super tezak kljuc'
        /// </remarks>
        /// <response code="200">Vraća listu reakcija</response>
        /// <response code="404">Nisu pronađene reakcije</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<List<ReactionsDto>> GetAllReactions([FromHeader(Name = "CommunicationKey")] string key)
        {
            var reactions = reactionRepository.GetReactions();

            if (reactions == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no reactions.");
            }

            return Ok(mapper.Map<List<ReactionsDto>>(reactions));

        }

        /// <summary>
        /// Vraća reakcije dodeljene specificiranom proizvodu
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// GET 'http://localhost:44360/reactions/byProductID' \
        /// Primer zahteva koji prolazi \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'productID = 1' \
        ///     --param  'userID = 4' \
        /// Primer zahteva koji ne prolazi jer je korisnik sa ID-jem 4 blokirao korisnika sa ID-jem 2, koji je vlasnik proizvoda sa ID-jem 2, i ne može videti njegove proizvode \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'productID = 2' \
        ///     --param  'userID = 4 
        /// </remarks>
        /// <param name="productID">ID proizvoda</param>
        /// <param name="userID">ID korisnika koji šalje zahtev</param>
        /// <response code="200">Vraća listu reakcija za specificirani proizvod</response>
        /// <response code="400">Loše kreiran zahtev</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađena reakcija sa zadatim ID-jem reakcije</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("byProductID")]
        public ActionResult GetReactionsByProductID([FromHeader(Name = "CommunicationKey")] string key, [FromQuery] int productID, [FromQuery] int userID)
        {

            if (productMockRepository.GetProductByID(productID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "There is no product with given ID");
            }

            var sellerID = productMockRepository.GetProductByID(productID).SellerID;

            // korisnik ne moze videti reakcije proizvoda cije vlasnike je on blokirao ili su njega blokirali
            if (reactionRepository.CheckDidIBlockedSeller(userID, sellerID) == true)
            {
                return StatusCode(StatusCodes.Status400BadRequest, String.Format("You can not see products with sellerID {0} ", sellerID));
            }

            //korisnik ne moze videti reakcije koje su dodali korisnici koje je on blokirao ili su njega blokirali
            var reactions = reactionRepository.GetRectionByProductID(productID, userID);

            if(reactions.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound, "This product has no reactions added");
            }

            return Ok(mapper.Map<List<ReactionsDto>>(reactions));
        }


        /// <summary>
        /// Kreira novu reakciju
        /// </summary>
        /// <param name="reaction">Model reakcije koji se dodaje</param>
        /// <param name="userID">ID korisnika koji pokreće zahtev</param>
        /// <returns></returns>
        /// <remarks>
        /// POST 'http://localhost:44360/reactions/' \
        /// Primer zahteva za kreiranje nove reakcije koji prolazi \
        ///  --header 'CommunicationKey: Super super tezak kljuc' \
        ///  --param 'userID = 4' \
        /// {     \
        ///  "productID": 1, \
        ///  "typeOfReactionID": 3 \
        /// } \
        /// Primer zahteva za kreiranje nove reakcije koji ne prolazi (ukoliko je prethodni zahtev izvršen), jer korisnik ne može dodati više od jedne reakcije na jedan proizvod \
        ///  --header 'CommunicationKey: Super super tezak kljuc' \
        ///  --param 'userID = 4' \
        /// {     \
        ///  "productID": 1, \
        ///  "typeOfReactionID": 2 \
        /// }  \
        ///  Primer zahteva za kreiranje novog komentara koji ne prolazi jer korisnik sa ID-jem 4 ne prati korisnika sa ID-jem 3, koji je vlasnik proizvoda sa ID-jem 3, i ne može dodavati reakcije na njegove proizvode \
        ///  --header 'CommunicationKey: Super super tezak kljuc' \
        ///  --param 'userID = 4' \
        /// {     \
        ///  "productID": 3, \
        ///  "typeOfReactionID": 3 \
        /// } 
        /// </remarks>
        /// <response code="201">Vraća potvrdu da je kreirana nova reakcija</response>
        /// <response code="400">Loše kreiran zahtev</response>
        /// <response code="401">Pogrešna autentifikacija</response>
        /// <response code="500">Greška na serveru prilikom čuvanja reakcije.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]
        public IActionResult AddReactionToProduct([FromHeader(Name = "CommunicationKey")] string key, [FromBody] ReactionCreateDto reaction, [FromQuery] int userID)
        {
            if(productMockRepository.GetProductByID(reaction.ProductID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Product with given ID does not exist");
            }

            if (typeOfReactionRepository.GetTypeOfReactionByID(reaction.TypeOfReactionID) == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Type of reaction with given ID does not exist!");
            }

            Reactions reactionEntity = mapper.Map<Reactions>(reaction);
            var product = productMockRepository.GetProductByID(reaction.ProductID);
            var sellerID = product.SellerID;

            if (reactionRepository.CheckDoIFollowSeller(userID, sellerID) == false)
            {
                return StatusCode(StatusCodes.Status400BadRequest, String.Format("You are not following user with id {0} and you can not add reaction to his products", sellerID));
            }

            if(reactionRepository.CheckUserWithProductID(userID, reaction.ProductID) != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "User can add only one reaction to specific product.");
            }

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

        /// <summary>
        /// Vrši izmenu jedne reakcije na osnovu ID-ja reakcije.
        /// </summary>
        /// <param name="reaction">Model reakcije koja se ažurira</param>
        /// <returns></returns>
        /// <remarks>
        /// Primer zahteva za modifikaciju reakcije  \
        ///  --header 'CommunicationKey: Super super tezak kljuc'  \
        /// PUT 'http://localhost:44360/reactions' \
        ///{ \
        /// "reactionID": "23209e86-e2a5-4691-d1e2-08d8c11a2ff5", \
        /// "productID": 4, \
        /// "typeOfReactionID": 3       \
        ///  }
        /// </remarks>
        /// <response code="200">Vraća potvrdu da je uspešno izmenjena reakcija</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađena reakcija za ažuriranje</response>
        /// <response code="500">Greška na serveru prilikom modifikacije reakcije</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut]
        public IActionResult UpdateReaction([FromHeader(Name = "CommunicationKey")] string key, [FromBody] ReactionUpdateDto reaction)
        {
            if(reactionRepository.GetReactionByID(reaction.ReactionID) == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "There is no reaction with given ID!");
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

        /// <summary>
        /// Vrši brisanje jedne reakcije na osnovu ID-ja reakcije.
        /// </summary>
        /// <param name="reactionID">ID reakcije koja se briše</param>
        /// <remarks>        
        /// Primer zahteva
        /// DELETE 'http://localhost:44360/reactions' \
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --param  'reactionID = 23209e86-e2a5-4691-d1e7-08d8c11a2ff7'
        /// </remarks>
        /// <returns>Status 204 (NoContent)</returns>
        /// <response code="204">Reakcija je uspešno obrisana</response>
        /// <response code="401">Greška pri autentifikaciji</response>
        /// <response code="404">Nije pronađena reakcija za brisanje</response>
        /// <response code="500">Došlo je do greške na serveru prilikom brisanja reakcije</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete]
        public IActionResult DeleteReacion([FromHeader(Name = "CommunicationKey")] string key, [FromQuery] Guid reactionID)
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
