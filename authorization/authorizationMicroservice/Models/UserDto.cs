using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Models
{
    /// <summary>
    /// Dto korisnika
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Id korisnika
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// Korisnicko ime
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Lozinka
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Salt vrednost korisnika
        /// </summary>
        public string Salt { get; set; }
    }
}
