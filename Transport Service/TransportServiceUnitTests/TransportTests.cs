using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Transport_Service.Controllers;
using Transport_Service.Models.DTOs;
using Transport_Service.Utils;
using TransportServiceUnitTests.Fakes;

namespace TransportServiceUnitTests
{
    [TestFixture]
    class TransportTests
    {
        private readonly TransportController transportController;
        public TransportTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"ServicesUrl:User", "https://localhost:44200/api/user"},
                {"ServicesUrl:Logger", "https://localhost:44200/api/logger"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            var logger = new FakeLogger(configuration);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(c => c.HttpContext.TraceIdentifier).Returns("1234");
            var tranposrtRepo = new FakeTransportRepository();
            var transportTypeRepo = new FakeTransportTypeRepository();
            transportController = new TransportController(logger, httpContextAccessorMock.Object, tranposrtRepo, transportTypeRepo);
        }

        [Test]
        public void GetTransportsByProvidedWeight_WeightNotProvided_BadRequest()
        {
            var response = transportController.GetAvailableTransportsByProvidedWeight(0);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public void GetTransportsByProvidedWeight_WeightProvided_OK()
        {
            var response = transportController.GetAvailableTransportsByProvidedWeight(5);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Test]
        public void CreateNewTransport_MinimalWeightGreaterThanMaximal_BadRequest()
        {
            TransportBodyDto transportBody = new TransportBodyDto
            {
                MinimalWeight = 15,
                MaximalWeight = 2,
                Price = 2,
                TransportTypeId = 1
            };

            var response = (BadRequestObjectResult)transportController.CreateNewTransport(transportBody);
            var body = JsonConvert.DeserializeObject<ResponseObject>(JsonConvert.SerializeObject(response.Value));
            Assert.AreEqual(body.Status, "Minimal weight cannot be greater or equal with minimal weight");
        }

        [Test]
        public void CreateNewTransport_MinimalWeightIsPartOfExistingRange_BadRequest()
        {
            TransportBodyDto transportBody = new TransportBodyDto
            {
                MinimalWeight = 200,
                MaximalWeight = 1500,
                Price = 2,
                TransportTypeId = 1
            };

            var response = (BadRequestObjectResult)transportController.CreateNewTransport(transportBody);
            var body = JsonConvert.DeserializeObject<ResponseObject>(JsonConvert.SerializeObject(response.Value));
            Assert.AreEqual(body.Status, "Minimal weight you are trying to set is part of existing range");
        }

        [Test]
        public void CreateNewTransport_MaximalWeightIsPartOfExistingRange_BadRequest()
        {
            TransportBodyDto transportBody = new TransportBodyDto
            {
                MinimalWeight = 0,
                MaximalWeight = 300,
                Price = 2,
                TransportTypeId = 1
            };

            var response = (BadRequestObjectResult)transportController.CreateNewTransport(transportBody);
            var body = JsonConvert.DeserializeObject<ResponseObject>(JsonConvert.SerializeObject(response.Value));
            Assert.AreEqual(body.Status, "Maximal weight you are trying to set is part of existing range");
        }

        [Test]
        public void CreateNewTransport_HappyScenario_OK()
        {
            TransportBodyDto transportBody = new TransportBodyDto
            {
                MinimalWeight = 5000,
                MaximalWeight = 5500,
                Price = 20,
                TransportTypeId = 1
            };

            var response = transportController.CreateNewTransport(transportBody);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);
            Assert.AreEqual(statusCode, HttpStatusCode.Created);
        }

        [Test]
        public void UpdateTransportDetails_UpdateThroughMultipleRangesLeft_BadRequest()
        {
            TransportBodyDto newTransport = new TransportBodyDto
            {
                Id = 4,
                MinimalWeight = 300,
                MaximalWeight = 2000,
                Price = 18,
                TransportTypeId = 1
            };

            var response = (BadRequestObjectResult)transportController.UpdateTransportDetails(4, newTransport);
            var body = JsonConvert.DeserializeObject<ResponseObject>(JsonConvert.SerializeObject(response.Value));
            Assert.AreEqual("Updating through multiple ranges not possible", body.Status);
        }

        [Test]
        public void UpdateTransportDetails_UpdateThroughMultipleRangesRight_BadRequest()
        {
            TransportBodyDto newTransport = new TransportBodyDto
            {
                Id = 1,
                MinimalWeight = 300,
                MaximalWeight = 2000,
                Price = 18,
                TransportTypeId = 1
            };

            var response = (BadRequestObjectResult)transportController.UpdateTransportDetails(4, newTransport);
            var body = JsonConvert.DeserializeObject<ResponseObject>(JsonConvert.SerializeObject(response.Value));
            Assert.AreEqual("Updating through multiple ranges not possible", body.Status);
        }

        [Test]
        public void UpdateTransportDetails_HappyScenario_OK()
        {
            TransportBodyDto newTransport = new TransportBodyDto
            {
                Id = 1,
                MinimalWeight = 300,
                MaximalWeight = 2000,
                Price = 18,
                TransportTypeId = 1
            };

            var response = transportController.UpdateTransportDetails(3, newTransport);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }

        [Test]
        public void DeleteTransport_ProvidedRangeWhichIsNotTheHighest_BadRequest()
        {
            var response = (BadRequestObjectResult)transportController.DeleteTransport(1);
            var body = JsonConvert.DeserializeObject<ResponseObject>(JsonConvert.SerializeObject(response.Value));
            Assert.AreEqual("Deleting of the range with the biggest values is only allowed", body.Status);
        }

        [Test]
        public void DeleteTransport_HappyScenario_OK()
        {
            var response = transportController.DeleteTransport(4);
            var statusCode = (HttpStatusCode)response
                                    .GetType()
                                    .GetProperty("StatusCode")
                                    .GetValue(response, null);

            Assert.AreEqual(statusCode, HttpStatusCode.OK);
        }
    }
}
