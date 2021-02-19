using NUnit.Framework;
using Recommendation_Service.Controllers;
using Recommendation_Service.Data;
using Recommendation_Service.Utils;
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
    class RecommendationControllerTests
    {
        private RecommendationControler recommendationControler;
        public RecommendationControllerTests()
        {
            
            var productService = new FakeProductService();
            var categoryRepository = new FakeCategoryRepository();
            recommendationControler = new RecommendationControler(productService, categoryRepository);
        }

        [Test]
        public async Task GetRecommededProducts_PriceNotProvided_NotFound()
        {
            var response = await recommendationControler.GetRecommededProducts(1, 0);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetRecommededProducts_HappyScenario_OK()
        {
            var response = await recommendationControler.GetRecommededProducts(1, 1000);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Test]
        public void CalculateCategoryPoints_ReturnsCorrectNumber_Success()
        {
            var points = Algorithm.CalculateCategoryPoints(3, 10);
            Assert.AreEqual(points, 20);
        }

        [Test]
        public void FindRecommendedCategory_ReturnsCorrectCategory_Success()
        {
            var categoryRepository = new FakeCategoryRepository();
            var categoryId = Algorithm.FindRecommendedCategory(categoryRepository.GetAllCategories(), 3, 10000);

            Assert.AreEqual(categoryId, 3);
        }
    }
}
