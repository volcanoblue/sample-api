using System.Net;
using System.Net.Http.Json;
using VolcanoBlue.SampleApi.Modules.Users.Shared;
using VolcanoBlue.SampleApi.Tests.Api.Fixture;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class CreateUserEndpointTests(ApiTestFixture fixture) : IClassFixture<ApiTestFixture>
    {
        [Fact]
        public async Task Should_return_201_when_command_is_valid()
        {
            //Arrange
            var activities = fixture.Factory.Telemetry.Traces;

            //Act
            var response = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            var user = await response.Content.ReadFromJsonAsync<UserCreatedResponse>();

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<Guid>(user!.Id);
            Assert.Contains(activities, act => act.DisplayName.Contains("POST /users"));
        }

        [Fact]
        public async Task Should_return_400_when_name_is_missing()
        {
            var response = await fixture.Client.PostAsJsonAsync("/users", new { name = string.Empty, email = "john@email.com" });
            var error = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(error);
            Assert.Contains("Name cannot be empty", error!);
        }

        [Fact]
        public async Task Should_return_404_when_email_is_missing()
        {
            var response = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = string.Empty });
            var error = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(error);
            Assert.Contains("Email cannot be empty", error!);
        }
    }
}
