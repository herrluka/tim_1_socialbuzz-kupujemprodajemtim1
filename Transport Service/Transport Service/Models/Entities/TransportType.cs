using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Transport_Service.Models.Entities
{
    /// <summary>
    /// Type of transport which determines which kind of vehicles
    /// are used to transport product
    /// </summary>
    [Table("TransportType")]
    public class TransportType
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Name of transport type
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
