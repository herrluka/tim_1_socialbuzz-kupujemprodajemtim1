using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Recommendation_Service.Data
{
    public class ProductService : IProductService
    {
        private readonly string Uri = "http://product-service.com/products";
        public async Task<List<ProductDto>> GetProductByCategoryAndPrice()
        {
            using (var client = new HttpClient())
            {
                var data = await client.GetStringAsync(Uri);
                var products = JsonSerializer.Deserialize<List<ProductDto>>(data);
                return products;
            }
            
        }
    }
}
