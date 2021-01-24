using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.Entities
{
    [Table("Transport")]
    public class Transport
    {
        [Key]
        public int Id { get; set; }
        [Range(0, Double.MaxValue, ErrorMessage = "Minimal weight must bre greater than 0")]
        public double MinimalWeight { get; set; }
        [Range(0, Double.MaxValue, ErrorMessage = "Maximal weight must bre greater than 0")]
        public double MaximalWeight { get; set; }
        public double Price { get; set; }
        [ForeignKey("TransportType")]
        [Column(name:"TransportType")]
        public int TransportTypeId { get; set; }
        public TransportType TransportType { get; set; }

    }
}
