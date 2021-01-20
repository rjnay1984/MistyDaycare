using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MistyDaycare.ApplicationCore.Constants;
using MistyDaycare.ApplicationCore.Extensions;
using MistyDaycare.PublicApi.Endpoints.Authorization;
using Xunit;

namespace Api.tests.FunctionalTests.PublicApi.Endpoints.Authorization
{
    [Collection("Sequential")]
    public class Authenticate : IClassFixture<ApiTestFixture>
    {
        public Authenticate(ApiTestFixture factory)
        {
            Client = factory.CreateClient();
        }

        public HttpClient Client { get; }

        [Theory]
        [InlineData("demouser@microsoft.com", AuthorizationConstants.DEFAULT_PASSWORD, HttpStatusCode.OK)]
        [InlineData("demouser@microsoft.com", "badpassword", HttpStatusCode.Unauthorized)]
        [InlineData("baduser@microsoft.com", "badpassword", HttpStatusCode.BadRequest)]
        public async Task ReturnsExpectedResultGivenCredentials(string testUsername, string testPassword, HttpStatusCode expectedResult)
        {
            var request = new AuthenticateRequest() 
            { 
                UserName = testUsername,
                Password = testPassword
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Client.PostAsync("api/authenticate", jsonContent);

            Assert.Equal(expectedResult, response.StatusCode);
        }   
    }
}