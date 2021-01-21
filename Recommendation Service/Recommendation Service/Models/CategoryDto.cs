using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Models
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category ID not provided")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name not provided")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Category rank not provided")]
        public int Rank { get; set; }

    }
}
