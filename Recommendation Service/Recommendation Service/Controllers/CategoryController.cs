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
