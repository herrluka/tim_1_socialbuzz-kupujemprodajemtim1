using authorizationMicroservice.Entities;
using authorizationMicroservice.Helpers;
using authorizationMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
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
                return Ok(new { token = tokenString });
            }
            return Unauthorized();
        }
        [HttpPost("admin")]
        public IActionResult AuthorizeAdmin(Principal principal)
        {
            if (AuthorizationHelper.ValidatePrincipal(principal,"admin"))
            {
                var tokenString = AuthorizationHelper.GenerateJWT(principal,"admin");
                return Ok(new { token = tokenString });
            }
            return Unauthorized();
        }
    }
}
