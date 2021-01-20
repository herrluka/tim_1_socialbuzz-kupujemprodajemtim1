using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Recommendation_Service.Data;
using Recommendation_Service.Utils;

namespace Recommendation_Service.Controllers
{
    /// <summary>
    /// Controler used to return list of products based on received product category and product price
    /// </summary>
    [ApiController]
    [Route("api")]
    public class RecommendationControler
    {
        private readonly IProductService productService;
        private readonly ApplicationDbContext applicationDbContext;
        private const double recommendedPercentOfPrice = 0.05;

        public RecommendationControler(IProductService productService, ApplicationDbContext applicationDbContext)
        {
            this.productService = productService;
            this.applicationDbContext = applicationDbContext;
        }


        [HttpGet("recommended-products")]
        public async Task<IActionResult> GetRecommededProducts([FromHeader] string token, [FromQuery] int productCategoryId, [FromQuery] double productPrice)
        {
            
            if (token is null)
            {
                return new UnauthorizedObjectResult(new { status = "Token not provided", content = (string)null });
            }

            var secretToken = Environment.GetEnvironmentVariable("API_SECRET_KEY");
            if (token != secretToken)
            {
                return new UnauthorizedObjectResult(new { status = "Bad token sent", content = (string)null });
            }

            var recommendedRank = Algorithm.FindRecommendedCategory(applicationDbContext, productCategoryId, productPrice);
            var recommendedPrice = productPrice + productPrice * recommendedPercentOfPrice;

            if (recommendedRank == null)
            {
                return new NotFoundObjectResult(new { status = "Bad category id sent", content = (string)null });
            }

            var products = await productService.GetProductByCategoryRankAndCeilingPrice((int)recommendedRank, recommendedPrice);

            return new OkObjectResult(new { status = "OK", content = products});
            
        }
    }
}
