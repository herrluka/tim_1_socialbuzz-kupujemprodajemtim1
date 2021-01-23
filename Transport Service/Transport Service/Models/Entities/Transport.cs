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
        public double MinimalWeight { get; set; }
        public double MaxWeight { get; set; }
        public double Price { get; set; }
        [ForeignKey("TransportType")]
        public int TransportTypeId { get; set; }
        public TransportType TransportType { get; set; }

    }
}
