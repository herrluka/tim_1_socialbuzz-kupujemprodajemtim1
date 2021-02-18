using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    /// Predstavlja model tipa reakcije
    /// </summary>
    public class TypeOfReaction
    {
        /// <summary>
        /// Id tipa reakcije
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
