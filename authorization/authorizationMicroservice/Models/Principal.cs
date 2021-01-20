using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace authorizationMicroservice.Models
{
    /// <summary>
    /// Principal za autorizaciju korisnika
    /// </summary>
    public class Principal
    {
        /// <summary>
        /// Korisnicko ime
        /// </summary>
        [Required(ErrorMessage ="Korisnicko ime je obavezno")]
        public string Username { get; set; }
        /// <summary>
        /// Lozinka
        /// </summary>
        [Required(ErrorMessage ="Lozinka je obavezna")]
        public string Password { get; set; }
    }
}
