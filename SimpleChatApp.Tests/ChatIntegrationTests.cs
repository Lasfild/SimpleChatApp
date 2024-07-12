using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SimpleChatApp.Tests.IntegrationTests
{
    public class StatusEndpointTests
    {
        private readonly HttpClient _client;

        public StatusEndpointTests()
        {
            // Создание HttpClient без использования testhost.deps.json
            _client = new HttpClient();
        }

        [Fact]
        public async Task Get_StatusEndpoint_ReturnsSuccessStatusCode()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/status");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
