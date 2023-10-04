using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using VanitiProject.Controllers;
using VanitiProject.Models;

namespace VanitiProject.Tests
{
    [TestClass]
    public class RatingControllerTests
    {
        /// <summary>
        /// Tests the AddRating() with an invalid rating value.
        /// </summary>
        [TestMethod]
        public async Task TestAddRating_InvalidRating()
        {
            // Arrange
            var mockHttpClient = new Mock<IHttpClientWrapper>();

            mockHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new List<Beer>
                    {
                        new Beer { Id = 1, Name = "Beer1" }
                    }))
                });

            var controller = new RatingController(mockHttpClient.Object);

            // Arrange
            var invalidRating = new Rating
            {
                UserName = "ff@ff.com",
                RatingValue = 0, // Updated to an invalid rating value
                Comments = "Nont Correct!"
            };

            // Act
            var result = await controller.AddRating(1, invalidRating) as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("'Rating Value' must be between 1 and 5. You entered 0.", result.Message);

        }

        /// <summary>
        /// Tests the AddRating() with a valid rating.
        /// </summary>
        [TestMethod]
        public async Task TestAddRating_ValidRating()
        {
            // Arrange
            var mockHttpClient = new Mock<IHttpClientWrapper>();

            mockHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new List<Beer>
                    {
                        new Beer { Id = 1, Name = "Beer1" }
                    }))
                });

            var controller = new RatingController(mockHttpClient.Object);

            // Arrange
            var validRating = new Rating
            {
                UserName = "user@example.com",
                RatingValue = 4,
                Comments = "Great beer!"
            };

            // Act
            var result = await controller.AddRating(1, validRating) as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Rating added successfully.", result?.Content);
        }
    }
}
