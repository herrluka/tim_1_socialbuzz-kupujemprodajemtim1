using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Models
{
    public class Principal
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
