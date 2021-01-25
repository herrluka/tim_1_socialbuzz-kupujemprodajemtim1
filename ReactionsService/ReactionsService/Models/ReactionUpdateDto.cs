using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    /// Dto za update Reaction-a
    /// </summary>
    public class ReactionUpdateDto
    {
        /// <summary>
        /// Id reakcije
        /// </summary>
        public Guid ReactionID { get; set; }

        /// <summary>
        /// Id proizvoda na koji je dodata reakcija
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Id tipa reakcije
        /// </summary>
        public int TypeOfReactionID { get; set; }

        
    }
}
