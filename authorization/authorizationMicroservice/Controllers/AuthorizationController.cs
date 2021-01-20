using authorizationMicroservice.Entities;
using authorizationMicroservice.Helpers;
using authorizationMicroservice.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Controllers
{
    [ApiController]
    [Route("api/authorize")]
    public class AuthorizationController:ControllerBase
    {
        private readonly IAuthorizationHelper AuthorizationHelper;
        private readonly ILogger logger;

        public AuthorizationController(IAuthorizationHelper authorizationHelper, ILogger logger)
        {
            AuthorizationHelper = authorizationHelper;
            this.logger = logger;
        }
        /// <summary>
        /// Autorizacija korisnika
        /// </summary>
        /// <param name="principal">Lozinka i korisnicko ime korisnika</param>
        /// <returns></returns>
        /// <remarks>
        /// POST 'https://localhost:5001/api/authorize/user' \
        /// --header 'Content-Type: application/json' \
        /// --data-raw '{
        /// "username":"UserUsername1",
        /// "password":"userPass1"
        /// }'
        /// </remarks>
        /// <response code="200">Vraca token za autentifikaciju korisnika</response>
        /// <response code="400">Lose struktuiran principal za autentifikaciju</response>
        /// <response code="401">Greska pri autentifikaciji komunikacije klijenta i servisa</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("user")]
        public IActionResult AuthorizeUser(Principal principal)
        {
           if(AuthorizationHelper.ValidatePrincipal(principal,"user"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"user");
                logger.Log(LogLevel.Warning, $"requestId: {Request.HttpContext.TraceIdentifier}, previousRequestId:No previous ID, Message: User {principal.Username} succesfully logged in");
                return Ok(new { token = tokenString });
            }
            logger.Log(LogLevel.Warning, $"requestId: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Invalid credentials from client");
            return Unauthorized();
        }
        /// <summary>
        /// Autorizacija admina
        /// </summary>
        /// <param name="principal">Lozinka i korisnicko ime admina</param>
        /// <returns>Token za autentifikaciju korisnika</returns>
        /// <remarks>
        /// /// POST 'https://localhost:5001/api/authorize/admin' \
        /// --header 'Content-Type: application/json' \
        /// --data-raw '{
        /// "username":"AdminUsername1",
        /// "password":"adminPass1"
        /// }'
        /// </remarks>
        /// <response code="200">Vraca token za autentifikaciju admina</response>
        /// <response code="400">Lose struktuiran principal za autentifikaciju</response>
        /// <response code="401">Greska pri autentifikaciji komunikacije klijenta i servisa</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("admin")]
        public IActionResult AuthorizeAdmin(Principal principal)
        {
            if (AuthorizationHelper.ValidatePrincipal(principal,"admin"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"admin");
                logger.Log(LogLevel.Warning, $"requestId: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Admin {principal.Username} succesfully logged in");
                return Ok(new { token = tokenString });
                 }
            logger.Log(LogLevel.Warning, $"requestId: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Invalid credentials from client");
            return Unauthorized();
        }
    }
}
