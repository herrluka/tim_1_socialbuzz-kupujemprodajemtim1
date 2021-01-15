using authorizationMicroservice.Entities;
using authorizationMicroservice.Helpers;
using authorizationMicroservice.Models;
using LoggingClassLibrary;
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

        public AuthorizationController(IAuthorizationHelper authorizationHelper)
        {
            AuthorizationHelper = authorizationHelper;
        }
        [HttpPost("user")]
        public IActionResult AuthorizeUser(Principal principal)
        {
           if(AuthorizationHelper.ValidatePrincipal(principal,"user"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"user");
                Logger.GetInstance().Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: User {principal.Username} succesfully logged in");
                return Ok(new { token = tokenString });
            }
            Logger.GetInstance().Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Invalid credentials from client");
            return Unauthorized();
        }
        [HttpPost("admin")]
        public IActionResult AuthorizeAdmin(Principal principal)
        {
            if (AuthorizationHelper.ValidatePrincipal(principal,"admin"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"admin");
                Logger.GetInstance().Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Admin {principal.Username} succesfully logged in");
                return Ok(new { token = tokenString });
                 }
            Logger.GetInstance().Log(LogLevel.Warning, $"RequestID: {Request.HttpContext.TraceIdentifier}, previousRequestID:No previous ID, Message: Invalid credentials from client");
            return Unauthorized();
        }
    }
}
