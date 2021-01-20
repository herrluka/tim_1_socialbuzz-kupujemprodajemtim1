using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName{ get; set; }
        public int  CategoryRank { get; set; }
    }
}
