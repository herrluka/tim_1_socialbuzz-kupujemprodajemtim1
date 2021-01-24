using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.Entities
{
    /// <summary>
    /// Database table used to stored informations about prices that depend on product weight
    /// for different types of transport
    /// </summary>
    [Table("Transport")]
    public class Transport
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Minimal weight of product which determines price of transporting for this product
        /// </summary>
        [Range(0, Double.MaxValue, ErrorMessage = "Minimal weight must bre greater than 0")]
        public double MinimalWeight { get; set; }
        /// <summary>
        /// Maximal weight of product which determines price of transporting for this product
        /// </summary>
        [Range(0, Double.MaxValue, ErrorMessage = "Maximal weight must bre greater than 0")]
        public double MaximalWeight { get; set; }
        /// <summary>
        /// Price for transporting product which has weight in this range
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Type of transport
        /// </summary>
        [ForeignKey("TransportType")]
        [Column(name:"TransportType")]
        public int TransportTypeId { get; set; }
        public TransportType TransportType { get; set; }

    }
}
