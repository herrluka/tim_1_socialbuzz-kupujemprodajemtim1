using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    // <summary>
    /// Dto za kreiranje tipa reakcije
    /// </summary>
    public class TypeOfReactionCreateDto
    {
        /// <summary>
        /// Naziv tipa reakcije
        /// </summary>
        [Required(ErrorMessage = "Ime reakcije je obavezno uneti!")]
        public String ReactionName { get; set; }

        /// <summary>
        /// Url do datog tipa reakcije
        /// </summary>
        [Required(ErrorMessage = "Url reakcije je obavezno uneti!")]
        public String Url { get; set; }
    }
}
