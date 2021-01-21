using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Recommendation_Service.Data;
using Recommendation_Service.Utils;
using Microsoft.Extensions.Logging;

namespace Recommendation_Service.Controllers
{
    /// <summary>
    /// Controler used to return list of products based on received product category and product price
    /// </summary>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    [Route("api")]
    public class RecommendationControler
    {
        private readonly IProductService productService;
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger logger;
        private const double recommendedPercentOfPrice = 0.05;

        public RecommendationControler(IProductService productService, ApplicationDbContext applicationDbContext, ILogger logger)
        {
            this.productService = productService;
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Return recommended products
        /// </summary>
        /// <param priceCategoryId="3">Product category id</param>
        /// <param productPrice="3000">Product price</param>
        /// <returns></returns>
        /// <remarks>
        /// An example of request
        /// GET 'http://localhost:52438/api/recommended-products/'
        ///     --header 'CommunicationKey: Super super tezak kljuc'
        ///     --header 'Content-Type: application/json'
        /// </remarks>
        /// <response code="200">Returns products list</response>
        /// <response code="400">Query params not provided / bad values</response>
        /// <response code="401">Token not provided or bad token provided</response>
        /// <response code="500">Unexpected error</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("recommended-products")]
        public async Task<IActionResult> GetRecommededProducts([FromQuery] int productCategoryId, [FromQuery] double productPrice)
        {

            if (productPrice == 0)
            {
                return new NotFoundObjectResult(new { status = "Price greater than zero not provided", content = (string)null });
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
