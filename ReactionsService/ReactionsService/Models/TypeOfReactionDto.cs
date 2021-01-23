using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    /// DTO za tip reakcije
    /// </summary>
    public class TypeOfReactionDto
    {

        /// <summary>
        /// Id tipa reakcije
        /// </summary>
        public Guid TypeOfReactionID { get; set; }

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
