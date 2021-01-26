using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    /// Dto za update tipa reakcije
    /// </summary>
    public class TypeOfReactionUpdateDto
    {
        /// <summary>
        /// ID tipa reakcije
        /// </summary>
        public int TypeOfReactionID { get; set; }

        /// <summary>
        /// Naziv tipa reakcije
        /// </summary>
        public String ReactionName { get; set; }

        /// <summary>
        /// Url do datog tipa reakcije
        /// </summary>
        public String Url { get; set; }
    }
}
