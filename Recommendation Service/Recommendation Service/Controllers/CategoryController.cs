using LoggingClassLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recommendation_Service.Data;
using Recommendation_Service.Data.Interfaces;
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
        private readonly ICategoryRepository categoryRepository;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly Logger logger;

        public CategoryController(ICategoryRepository categoryRepository , Logger logger, IHttpContextAccessor contextAccessor)
        {
            this.categoryRepository = categoryRepository;
            this.logger = logger;
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <returns></returns>
        /// <remarks>
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
            
            try
            {
                categoryRepository.CreateNewCategory(newCategory);
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully created Category record {0} in database", category.Name), null);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Record with name {0} not created, reason - {1}", category.Name, ex.Message), null);
                return new BadRequestObjectResult(new
                {
                    status = "Saving in database not successful",
                    content = (string)null
                });
            }

            return new OkObjectResult(new
            {
                status = "Successfully created",
                content = (string)null
            })
            {
                StatusCode = StatusCodes.Status201Created
            };

        }

        /// <summary>
        /// Update existing category
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// PUT 'http://localhost:52438/api/category/'
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --header 'Content-Type: application/x-www-form-urlencoded' \
        ///     --data-urlencode 'Id=7' \
        ///     --data-urlencode 'Name=Racunari i laptopovi' \
        ///     --data-urlencode 'Rank=4' \
        /// </remarks>
        /// <response code="200">Record successfully updated</response>
        /// <response code="400">Saving in database not successful</response>
        /// <response code="401">Token not provided or bad token provided</response>
        /// <response code="404">Existing category not found</response>
        /// <response code="415">Bad body data sent</response>
        /// <response code="500">Unexpected error on server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPut("category/{id}")]
        public IActionResult UpdateExistingCategory(int id, CategoryDto category)
        {
            var existingCategory = categoryRepository.GetCategoryById(id);

            if (existingCategory == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = "Provided category not found",
                    content = (string)null
                });
            }

            existingCategory.CategoryName = category.Name;
            existingCategory.CategoryRank = category.Rank;
           
            try
            {
                categoryRepository.UpdateCategory(existingCategory);
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully updated Category record with id {0} in database", category.Id), null);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Record with id {0} not updated, reason - {1}", category.Id, ex.Message), null);
                return new BadRequestObjectResult(new
                {
                    status = "Saving in database not successful",
                    content = (string)null
                });
            }

            return new OkObjectResult(new
            {
                status = "Successfully updated",
                content = (string)null
            });

        }

        /// <summary>
        /// Delete existing category
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// DELETE 'http://localhost:52438/api/category/'
        ///     --header 'CommunicationKey: Super super tezak kljuc' \
        ///     --header 'Content-Type: application/x-www-form-urlencoded' \
        ///     --data-urlencode 'Id=7' \
        /// </remarks>
        /// <response code="200">Record successfully deleted</response>
        /// <response code="400">Saving in database not successful</response>
        /// <response code="401">Token not provided or bad token provided</response>
        /// <response code="404">Existing category not found</response>
        /// <response code="500">Unexpected error on server</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpDelete("category/{id}")]
        public IActionResult DeleteExistingCategory(int id)
        {
            var existingCategory = categoryRepository.GetCategoryById(id);

            if (existingCategory == null)
            {
                return new NotFoundObjectResult(new
                {
                    status = "Provided category not found",
                    content = (string)null
                });
            }

            try
            {
                categoryRepository.DeleteCategory(existingCategory);
                logger.Log(LogLevel.Information, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Successfully deleted Category record with id {0} in database", id), null);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, contextAccessor.HttpContext.TraceIdentifier, "", String.Format("Record with id {0} not deleted, reason - {1}", id, ex.Message), null);
                return new BadRequestObjectResult(new
                {
                    status = "Saving in database not successful",
                    content = (string)null
                });
            }

            return new OkObjectResult(new
            {
                status = "Successfully deleted",
                content = (string)null
            });

        }
    }
}
