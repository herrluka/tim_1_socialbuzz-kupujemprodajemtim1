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
        private readonly string BaseUri = Environment.GetEnvironmentVariable("PRODUCT_SERVICE_BASE_URL");
        private readonly string EndpointUri = "products/";

        public async Task<List<ProductDto>> GetProductsByCategoryRankAndCeilingPrice(int categoryRank, double productCeilingPrice)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", Environment.GetEnvironmentVariable("PRODUCT_SERVICE_SECRET_KEY"));

                UriBuilder builder = new UriBuilder(BaseUri + EndpointUri);
                builder.Query = "categoryRank=" + categoryRank + "&price=" + productCeilingPrice;
                var data = await client.GetStringAsync(builder.Uri);
                var products = JsonSerializer.Deserialize<List<ProductDto>>(data);
                return products;
            }
            
        }
    }
}
