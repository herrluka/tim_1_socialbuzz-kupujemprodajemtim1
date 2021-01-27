using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommentingService.Model
{
    /// <summary>
    /// Dto za model komentara koji se dodaje na objavu 
    /// </summary>
    public class CommentsDto
    {
        /// <summary>
        /// Id komentara
        /// </summary>
        public Guid CommentID { get; set; }

        /// <summary>
        /// Id proizvoda na koji je dodat komentar
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Id korisnika koji je dodao komentar
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Sadrzaj datog komentara
        /// </summary>
        [Required(ErrorMessage = "Tekst komentara je obavezno uneti!")]
        public String Content { get; set; }

        /// <summary>
        /// Datum kada je dodat dati komentar
        /// </summary>
        [Required(ErrorMessage = "Datum komentara je obavezno uneti!")]
        public DateTime CommentDate { get; set; }
    }
}
