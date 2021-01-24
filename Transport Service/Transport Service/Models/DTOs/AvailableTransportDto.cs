using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    public class AvailableTransportDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Transport type field is required")]
        public string TransportType { get; set; }
        [Required(ErrorMessage = "Price field is required")]
        public double Price { get; set; }
    }
}
