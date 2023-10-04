using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using VanitiProject.Controllers;
using VanitiProject.Models;

namespace VanitiProject.Tests
{
    [TestClass]
    public class BeersControllerTests
    {
        /// <summary>
        /// Tests the GetBeers method with an invalid search.
        /// </summary>
        [TestMethod]
        public async Task TestGetBeers_WithInvalidSearch()
        {
            // Arrange
            var mockHttpClient = new Mock<IHttpClientWrapper>();

            mockHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new List<Beer>()))
                });

            var controller = new BeersController(mockHttpClient.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetBeers("invalid-search");

            // Assert
            Assert.IsNotNull(actionResult, "Expected result not to be null.");
        }

        /// <summary>
        /// Tests the GetBeers method with a valid search.
        /// </summary>
        [TestMethod]
        public async Task TestGetBeers_WithValidSearch()
        {
            // Arrange
            var mockHttpClient = new Mock<IHttpClientWrapper>();

            mockHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new List<Beer>()))
                });

            var controller = new BeersController(mockHttpClient.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetBeers("valid-search");

            // Assert
            Assert.IsNotNull(actionResult, "Expected result not to be null.");
        }
    }
}
