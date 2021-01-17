using authorizationMicroservice.Entities;
using authorizationMicroservice.Helpers;
using authorizationMicroservice.Models;
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
        [HttpPost("user")]
        public IActionResult AuthorizeUser(Principal principal)
        {
           if(AuthorizationHelper.ValidatePrincipal(principal,"user"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"user");
                logger.Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: User {principal.Username} succesfully logged in");
                return Ok(new { token = tokenString });
            }
            logger.Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Invalid credentials from client");
            return Unauthorized();
        }
        [HttpPost("admin")]
        public IActionResult AuthorizeAdmin(Principal principal)
        {
            if (AuthorizationHelper.ValidatePrincipal(principal,"admin"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"admin");
                logger.Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Admin {principal.Username} succesfully logged in");
                return Ok(new { token = tokenString });
                 }
            logger.Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Invalid credentials from client");
            return Unauthorized();
        }
    }
}
