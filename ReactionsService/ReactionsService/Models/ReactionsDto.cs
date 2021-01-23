using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    /// DTO reakcije
    /// </summary>
    public class ReactionsDto
    {
        /// <summary>
        /// Id reakcije
        /// </summary>
        public int ReactionID { get; set; }

        /// <summary>
        /// Id proizvoda na koji je dodata reakcija
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Id tipa reakcije
        /// </summary>
        public int TypeOfReactionID { get; set; }

        /// <summary>
        /// Id usera koji je dodao reakciju
        /// </summary>
        public int UserID { get; set; }
    }
}
