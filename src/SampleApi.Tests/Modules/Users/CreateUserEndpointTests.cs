using System.Net;
using System.Net.Http.Json;
using VolcanoBlue.SampleApi.Tests.Api.Fixture;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class CreateUserEndpointTests(ApiTestFixture fixture) : IClassFixture<ApiTestFixture>
    {
        [Fact]
        public async Task Should_have_tracing_activity_when_command_is_valid()
        {
            //Arrange
            var spans = fixture.Factory.Telemetry.Traces;

            //Act
            var response = await fixture.Client.PostAsJsonAsync(Endpoints.CreateUser, new { name = "John", email = "john@email.com" });
            var user = await response.Content.ReadFromJsonAsync<UserCreatedResponse>();
            await Task.Delay(100); // Give some time for the telemetry to be recorded

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<Guid>(user!.Id);
            Assert.Contains(spans, span => span.DisplayName.Contains($"POST {Endpoints.CreateUser}"));
        }

        [Fact]
        public async Task Should_return_201_when_command_is_valid()
        {
            //Act
            var response = await fixture.Client.PostAsJsonAsync(Endpoints.CreateUser, new { name = "John", email = "john@email.com" });
            var user = await response.Content.ReadFromJsonAsync<UserCreatedResponse>();

            //Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.IsType<Guid>(user!.Id);
        }

        [Fact]
        public async Task Should_return_400_when_name_is_missing()
        {
            var response = await fixture.Client.PostAsJsonAsync(Endpoints.CreateUser, new { name = string.Empty, email = "john@email.com" });
            var error = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(error);
        }

        [Fact]
        public async Task Should_return_400_when_email_is_missing()
        {
            var response = await fixture.Client.PostAsJsonAsync(Endpoints.CreateUser, new { name = "John", email = string.Empty });
            var error = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotNull(error);
        }
    }
}
