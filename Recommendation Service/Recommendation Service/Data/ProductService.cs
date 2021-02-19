using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Recommendation_Service.Data
{
    public class ProductService : IProductService
    {
        private readonly string Uri = "http://product-service.com/products";
        public async Task<List<ProductDto>> GetProductsByCategoryRankAndCeilingPrice(int categoryRank, double productCeilingPrice)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("PRODUCT_SERVICE_SECRET_KEY"));

                UriBuilder builder = new UriBuilder(Uri);
                builder.Query = "categoryRank=" + categoryRank + "&price=" + productCeilingPrice;
                var data = await client.GetStringAsync(Uri);
                var products = JsonSerializer.Deserialize<List<ProductDto>>(data);
                return products;
            }
            
        }
    }
}
