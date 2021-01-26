using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReactionsService.Models
{
    /// <summary>
    /// DTO za kreiranje Reakcije
    /// </summary>
    public class ReactionCreateDto
    {
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
