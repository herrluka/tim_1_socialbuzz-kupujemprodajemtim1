using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    /// <summary>
    /// Data transfer object which contains information about
    /// currently available transports for product with
    /// particular weight
    /// </summary>
    public class AvailableTransportDto
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of transport type
        /// </summary>
        [Required(ErrorMessage = "Transport type field is required")]
        public string TransportType { get; set; }
        /// <summary>
        /// Price for this type of transport
        /// </summary>
        [Required(ErrorMessage = "Price field is required")]
        public double Price { get; set; }
    }
}
