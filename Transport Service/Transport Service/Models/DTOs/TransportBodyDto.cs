using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    public class TransportBodyDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Price field is required")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Minimal weight property is required")]
        [Range(0, Double.MaxValue, ErrorMessage = "Maximal weight must bre greater than 0")]
        public double MinimalWeight { get; set; }
        [Required(ErrorMessage = "Maximal weight property is required")]
        [Range(0, Double.MaxValue, ErrorMessage = "Maximal weight must bre greater than 0")]
        public double MaximalWeight { get; set; }
        [Required(ErrorMessage = "Transport type property is required")]
        public int TransportTypeId { get; set; }
    }
}
