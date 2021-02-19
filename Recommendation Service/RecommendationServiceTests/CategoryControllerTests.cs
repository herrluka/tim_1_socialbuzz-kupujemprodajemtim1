using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Recommendation_Service.Controllers;
using Recommendation_Service.Data.Fakes;
using Recommendation_Service.Models;
using RecommendationServiceTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationServiceTests
{
    [TestFixture]
    class CategoryControllerTests
    {
        private CategoryController categoryController;
        public CategoryControllerTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"ServicesUrl:User", "https://localhost:44200/api/user"},
                {"ServicesUrl:Logger", "https://localhost:44200/api/logger"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(c => c.HttpContext.TraceIdentifier).Returns("1234");

            var categoryRepository = new FakeCategoryRepository();
            var logger = new FakeLoggerService(configuration);
            categoryController = new CategoryController(categoryRepository, logger, httpContextAccessorMock.Object);
        }

        [Test]
        public void CreateNewCategory_ErrorSavingInDBBecauseIdAlreadyExists_BadRequest()
        {
            var categoryDTO = new CategoryDto()
            {
                Id = 1,
                Name = "Test1",
                Rank = 2
            };

            var response = categoryController.CreateNewCategory(categoryDTO);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void CreateNewCategory_HappyScenario_OK()
        {
            var categoryDTO = new CategoryDto()
            {
                Id = 20,
                Name = "Test20",
                Rank = 2
            };

            var response = categoryController.CreateNewCategory(categoryDTO);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Test]
        public void UpdateExistingCategory_BadIdProvided_BadRequest()
        {
            var categoryDTO = new CategoryDto()
            {
                Id = 20,
                Name = "Test20",
                Rank = 2
            };

            var response = categoryController.UpdateExistingCategory(20, categoryDTO);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void UpdateExistingCategory_HappyScenario_OK()
        {
            var categoryDTO = new CategoryDto()
            {
                Id = 4,
                Name = "Test20",
                Rank = 2
            };

            var response = categoryController.UpdateExistingCategory(4, categoryDTO);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Test]
        public void DeleteExistingCategory_BadCategoryIdProvided_NotFound()
        {
            var response = categoryController.DeleteExistingCategory(20);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);

            Assert.AreEqual(statusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public void DeleteExistingCategory_HappyScenario_OK()
        {
            var response = categoryController.DeleteExistingCategory(2);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }
    }
}
