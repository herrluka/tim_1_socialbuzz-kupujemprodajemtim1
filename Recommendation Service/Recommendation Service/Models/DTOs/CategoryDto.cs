using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Models
{
    /// <summary>
    /// Category data transfer object used for
    /// communication with Product service
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Unique identifier of Category
        /// </summary>
        [Required(ErrorMessage = "Category ID not provided")]
        public int Id { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        [Required(ErrorMessage = "Category name not provided")]
        public string Name { get; set; }
        /// <summary>
        /// Category rank
        /// </summary>
        [Required(ErrorMessage = "Category rank not provided")]
        public int Rank { get; set; }

    }
}
