using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    public class TransportTypeDto
    {
        [Required(ErrorMessage = "You must provide transport type name")]
        public string Name { get; set; }
    }
}
