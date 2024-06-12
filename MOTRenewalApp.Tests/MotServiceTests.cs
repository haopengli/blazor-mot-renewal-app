using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using MotRenewalApp.Models;
using MotRenewalApp.Services;
using Xunit;

namespace MOTRenewalApp.Tests
{
    public class MotServiceTests
    {
        private HttpClient CreateHttpClient(Mock<HttpMessageHandler> mockHttpMessageHandler)
        {
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://any.url")
            };
            return httpClient;
        }

        [Fact]
        public async Task FetchMotData_ReturnsCorrectVehicleData()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new List<VehicleDto>
                    {
                    new VehicleDto
                    {
                        Make = "Toyota",
                        Model = "Prius",
                        PrimaryColour = "Red",
                        MotTests = new List<MotTestDto>
                        {
                            new MotTestDto { ExpiryDate = "2024.07.04", OdometerValue = "40413" }
                        }
                    }
                    })
                });

            var httpClient = CreateHttpClient(mockHttpMessageHandler);
            var settings = Options.Create(new MotApiSettings
            {
                Url = "/trade/vehicles/mot-tests?registration=",
                ApiKey = "apiKey",
            });

            var motService = new MotService(httpClient, settings);

            // Act
            var vehicle = await motService.FetchMotData("XX10ABC");

            // Assert
            Assert.NotNull(vehicle);
            Assert.Equal("Toyota", vehicle.Make);
            Assert.Equal("Prius", vehicle.Model);
            Assert.Equal("Red", vehicle.Colour);
            Assert.Equal(new DateTime(2024, 7, 4), vehicle.ExpiryDate);
            Assert.Equal("40413", vehicle.MileageLastMot);
        }

        [Fact]
        public async Task FetchMotData_HandlesNotFound()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            var httpClient = CreateHttpClient(mockHttpMessageHandler);
            var settings = Options.Create(new MotApiSettings
            {
                Url = "/trade/vehicles/mot-tests?registration=",
                ApiKey = "apiKey",
            });

            var motService = new MotService(httpClient, settings);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => motService.FetchMotData("XX10ABC"));
            Assert.Equal("The number plate was not found. Please check the registration number and try again.", exception.Message);
        }

        [Fact]
        public async Task FetchMotData_HandlesHttpRequestException()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            var httpClient = CreateHttpClient(mockHttpMessageHandler);
            var settings = Options.Create(new MotApiSettings
            {
                Url = "/trade/vehicles/mot-tests?registration=",
                ApiKey = "apiKey",
            });

            var motService = new MotService(httpClient, settings);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => motService.FetchMotData("XX10ABC"));
            Assert.Equal("There was a problem connecting to the MOT service. Please try again later.", exception.Message);
        }

        [Fact]
        public async Task FetchMotData_HandlesEmptyResponse()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[]") // Empty JSON array
                });

            var httpClient = CreateHttpClient(mockHttpMessageHandler);
            var settings = Options.Create(new MotApiSettings
            {
                Url = "/trade/vehicles/mot-tests?registration=",
                ApiKey = "apiKey",
            });

            var motService = new MotService(httpClient, settings);

            // Act
            var vehicle = await motService.FetchMotData("XX10ABC");

            // Assert
            Assert.Null(vehicle);
        }
    }
}