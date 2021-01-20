using Microsoft.AspNetCore.Mvc;
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
    [Route("category")]
    public class CategoryController
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        [HttpPost()]
        public IActionResult CreateNewCategory([FromHeader] string token, [FromBody] CategoryDto category)
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

            var newCategory = new Category()
            {
                Id = category.Id,
                CategoryName = category.Name,
                CategoryRank = category.Rank
            };

            applicationDbContext.Category.Attach(newCategory);

            try
            {
                applicationDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { status = "Saving in database not successful", 
                                                        content = (string)null });
            }

            return new OkObjectResult(new
            {
                status = "Successfully saved",
                content = (string)null
            });

        }
    }
}
