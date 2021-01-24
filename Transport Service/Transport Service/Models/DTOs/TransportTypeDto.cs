using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    /// <summary>
    /// Data transfer object which contains information
    /// about available transport types
    /// </summary>
    public class TransportTypeDto
    {
        /// <summary>
        /// Name of transport type
        /// </summary>
        [Required(ErrorMessage = "You must provide transport type name")]
        public string Name { get; set; }
    }
}
