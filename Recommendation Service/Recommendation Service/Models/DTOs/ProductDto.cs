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
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime DateCreated { get; set; }
        public string Location { get; set; }
        public int CategoryId { get; set; }
    }
}
