using ComplytekTest.API.Infrastructure.Services.External;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ComplytekTest.API.Test.InfrastructureServices
{
    public class RandomStringGeneratorServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<RandomStringGeneratorService>> _loggerMock;

        public RandomStringGeneratorServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<RandomStringGeneratorService>>();
        }

        [Fact]
        public async Task GenerateAsync_ReturnsCode_WhenApiResponseIsValid()
        {
            // Arrange
            var expectedCode = "Abc123@!";
            var apiUrl = "https://api.example.com";
            _configurationMock.Setup(c => c["RandomStringApiUrl"]).Returns(apiUrl);

            var responseContent = JsonSerializer.Serialize(new[] { expectedCode });
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var service = new RandomStringGeneratorService(_httpClient, _configurationMock.Object, _loggerMock.Object);

            // Act
            var result = await service.GenerateAsync();

            // Assert
            Assert.Equal(expectedCode, result);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenApiUrlMissing()
        {
            // Arrange
            _configurationMock.Setup(c => c["RandomStringApiUrl"]).Returns((string)null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new RandomStringGeneratorService(_httpClient, _configurationMock.Object, _loggerMock.Object));
            Assert.Contains("RandomStringApiUrl not found", ex.Message);
        }
    }
}