using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Models
{
    /// <summary>
    /// Data transfer object used for communication
    /// with Product service
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Product unique ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Product price
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// Product creation date
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Where product is offered
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// Category which product belongs
        /// </summary>
        public int CategoryId { get; set; }
    }
}
