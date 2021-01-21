using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recommendation_Service.Data;
using Recommendation_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommendation_Service.Controllers
{
    /// <summary>
    /// Controler used to take care of synchornizing Category table
    /// </summary>
    [ApiController]
    [Route("api")]
    public class CategoryController
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Primer unosa novog loga \
        /// POST 'http://localhost:52438/api/category/'
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --header 'Content-Type: application/x-www-form-urlencoded' \
        ///     --data-urlencode 'Id=11' \
        ///     --data-urlencode 'Name=Dronovi' \
        ///     --data-urlencode 'Rank=3' \
        /// </remarks>
        /// <response code="201">Record successfully created</response>
        /// <response code="400">Saving in database not successful</response>
        /// <response code="401">Token not provided or bad token provided</response>
        /// <response code="415">Bad body data sent</response>
        /// <response code="500">Unexpected error on server</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("category")]
        public IActionResult CreateNewCategory([FromBody] CategoryDto category)
        {
            
            var newCategory = new Category()
            {
                Id = category.Id,
                CategoryName = category.Name,
                CategoryRank = category.Rank
            };

            applicationDbContext.Category.Add(newCategory);

            try
            {
                applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new
                {
                    status = "Saving in database not successful",
                    content = (string)null
                });
            }

            return new OkObjectResult(new
            {
                status = "Successfully saved",
                content = (string)null
            });

        }
    }
}
