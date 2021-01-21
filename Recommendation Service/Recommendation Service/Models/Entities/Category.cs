using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Models
{
    /// <summary>
    /// Model that represent database class Category
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        public string CategoryName{ get; set; }
        /// <summary>
        /// Category rank calculated based on average price
        /// </summary>
        public int  CategoryRank { get; set; }
    }
}
