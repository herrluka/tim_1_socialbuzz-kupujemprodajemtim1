using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Data
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProductByCategoryRankAndCeilingPrice(int categoryRank, double productCeilingPrice);
    }
}
