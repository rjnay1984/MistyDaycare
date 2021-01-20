using MistyDaycare.ApplicationCore.Constants;
using MistyDaycare.PublicApi;
using MistyDaycare.PublicApi.Endpoints.Authorization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace MistyDaycare.FunctionalTests.PublicApi.Endpoints.Authorization
{
    [Collection("Sequential")]
    public class Register : IClassFixture<ApiTestFixture>
    {
        public Register(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Theory]
        [InlineData("newuser@microsoft.com", AuthorizationConstants.DEFAULT_PASSWORD, HttpStatusCode.OK)]
        [InlineData("demouser@microsoft.com", AuthorizationConstants.DEFAULT_PASSWORD, HttpStatusCode.BadRequest)] // Existing user
        [InlineData("baduser@microsoft.com", "fail", HttpStatusCode.BadRequest)]
        public async Task ReturnsExpectedStatusCodeGivenCredentials(string userName, string password, HttpStatusCode statusCode)
        {
            var request = new RegisterRequest()
            {
                UserName = userName,
                Password = password
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/register", jsonContent);

            Assert.Equal(statusCode, response.StatusCode);
        }
    }
}
