using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.DTOs
{
    /// <summary>
    /// Model of body JSON object required for creation
    /// of new record which contains information about
    /// transport type, product weight range & price for 
    /// this kind of transport
    /// </summary>
    public class TransportBodyDto
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Price for this kind of transport
        /// </summary>
        [Required(ErrorMessage = "Price field is required")]
        public double Price { get; set; }
        /// <summary>
        /// Minimal weight of product which determines weight range where
        /// above mentioned price is applied
        /// </summary>
        [Required(ErrorMessage = "Minimal weight property is required")]
        [Range(0, Double.MaxValue, ErrorMessage = "Maximal weight must bre greater than 0")]
        public double MinimalWeight { get; set; }
        /// <summary>
        /// Maximal weight of product which determines weight range where
        /// above mentioned price is applied
        /// </summary>
        [Required(ErrorMessage = "Maximal weight property is required")]
        [Range(0, Double.MaxValue, ErrorMessage = "Maximal weight must bre greater than 0")]
        public double MaximalWeight { get; set; }
        /// <summary>
        /// Foreign key for TansportType table
        /// </summary>
        [Required(ErrorMessage = "Transport type property is required")]
        public int TransportTypeId { get; set; }
    }
}
