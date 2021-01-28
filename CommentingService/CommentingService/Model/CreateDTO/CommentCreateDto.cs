using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model.CreateDTO
{
    /// <summary>
    /// Dto za kreiranje komentara
    /// </summary>
    public class CommentCreateDto
    {
        
        /// <summary>
        /// Id proizvoda na koji je dodat komentar
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Sadrzaj datog komentara
        /// </summary>
        [Required(ErrorMessage = "Tekst komentara je obavezno uneti!")]
        public String Content { get; set; }



    }
}
