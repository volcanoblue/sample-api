using System.Net;
using System.Net.Http.Json;
using VolcanoBlue.SampleApi.Tests.Api.Fixture;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class CreateUserEndpointTests(ApiTestFixture fixture) : IClassFixture<ApiTestFixture>
    {
        [Fact]
        public async Task Should_return_201_when_command_is_valid()
        {
            //Arrange
            var spans = fixture.Factory.Telemetry.Traces;

            //Act
            var response = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            
            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Single(fixture.Factory.UserRepository.GetAll());
            Assert.Contains(spans, span => span.DisplayName.Contains("POST /users"));
        }
    }
}
