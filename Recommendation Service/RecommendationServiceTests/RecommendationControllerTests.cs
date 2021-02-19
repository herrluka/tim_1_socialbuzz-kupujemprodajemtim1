using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Recommendation_Service.Controllers;
using Recommendation_Service.Data;
using Recommendation_Service.Data.Fakes;
using RecommendationServiceTests.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var inMemorySettings = new Dictionary<string, string> {
                {"ServicesUrl:User", "https://localhost:44200/api/user"},
                {"ServicesUrl:Logger", "https://localhost:44200/api/logger"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            
            var productService = new FakeProductService();
            var categoryRepository = new FakeCategoryRepository();
            recommendationControler = new RecommendationControler(productService, categoryRepository);
        }

        [Test]
        public void test1()
        {
            Assert.Pass();
        }
    }
}
