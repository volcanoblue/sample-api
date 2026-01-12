using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using System.Net;
using System.Net.Http.Json;
using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Tests.Api.Fixture;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class ChangeEmailEndpointTests(ApiTestFixture fixture) : IClassFixture<ApiTestFixture>
    {
        [Fact]
        public async Task Should_return_204_when_command_is_valid()
        {
            //Arrange
            var userRepository = fixture.Factory.Services.GetService<IUserRepository>();
            var userCreated = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>();
            var newEmail = "john.doe@email.com";
            var ct = CancellationToken.None;

            //Act
            var emailChanged = await fixture.Client.PatchAsJsonAsync("/users", new { id = user!.Id, newemail = newEmail });
            
            //Assert
            Assert.Equal(HttpStatusCode.NoContent, emailChanged.StatusCode);
            Assert.Equal(newEmail, (await userRepository!.GetByIdAsync(user.Id, ct)).ResultValue.Email);
        }

        [Fact]
        public async Task Should_increment_counter_when_command_is_valid()
        {
            //Arrange
            var userCreated = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>();
            var newEmail = "john.doe@email.com";

            //Act
            var emailChanged = await fixture.Client.PatchAsJsonAsync("/users", new { id = user!.Id, newemail = newEmail });

            //Assert
            await fixture.Factory.FlushMetricsAsync();
            var metrics = fixture.Factory.Telemetry.Metrics;
            var emailChangedMetric = metrics.FirstOrDefault(m => m.Name == ChangeEmailMetrics.UsersEmailChangedCounterName);

            Assert.NotNull(emailChangedMetric);
            Assert.InRange(emailChangedMetric.GetCounterValue(), 1, 2);
        }

        [Fact]
        public async Task Should_return_400_when_email_is_invalid()
        {
            //Arrange
            var userCreated = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>();
            
            //Act
            var response = await fixture.Client.PatchAsJsonAsync("/users", new { id = user!.Id, newemail = string.Empty });
            var message = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Email cannot be empty", message);
        }

        [Fact]
        public async Task Should_return_404_when_user_wasnt_found()
        {
            //Act
            var response = await fixture.Client.PatchAsJsonAsync("/users", new { id = Guid.Empty, newemail = string.Empty });
            var message = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Contains("User not found", message);
        }
    }

    public sealed record UserCreatedResponse(Guid Id);

    public static class MetricExtensions
    {
        public static long GetCounterValue(this Metric metric)
        {
            foreach (ref readonly var metricPoint in metric.GetMetricPoints())
                return metricPoint.GetSumLong();

            return 0;
        }
    }
}
