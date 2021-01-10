using authorizationMicroservice.Entities;
using authorizationMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Helpers
{
    public interface IAuthorizationHelper
    {
        public string GenerateJWT(Principal principal,string type);
        public Task<User> GetUser(Principal principal,string type);
        public bool ValidatePrincipal(Principal principal,string type);
    }
}
