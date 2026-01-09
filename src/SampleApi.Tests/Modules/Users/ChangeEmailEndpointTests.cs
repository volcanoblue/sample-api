using OpenTelemetry.Metrics;
using System.Net;
using System.Net.Http.Json;
using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Tests.Api.Fixture;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class ChangeEmailEndpointTests(ApiTestFixture fixture) : IClassFixture<ApiTestFixture>
    {
        [Fact]
        public async Task Should_return_200_when_command_is_valid()
        {
            //Arrange
            var userCreated = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>();
            var newEmail = "john.doe@email.com";
            var ct = CancellationToken.None;

            //Act
            var emailChanged = await fixture.Client.PostAsJsonAsync("/users/change-email", new { id = user!.Id, newemail = newEmail });
            
            //Assert
            Assert.Equal(HttpStatusCode.OK, emailChanged.StatusCode);
            Assert.Equal(newEmail, fixture.Factory.UserRepository.GetByIdAsync(user.Id, ct).ResultValue.Get().Email);

            await fixture.Factory.FlushMetricsAsync();
            var metrics = fixture.Factory.Telemetry.Metrics;
            var emailChangedMetric = metrics.FirstOrDefault(m => m.Name == ChangeEmailMetrics.UsersEmailChangedCounterName);

            Assert.NotNull(emailChangedMetric);
            Assert.Equal(1, emailChangedMetric.GetCounterValue());
        }

        [Fact]
        public async Task Should_increment_counter_when_command_is_valid()
        {
            //Arrange
            var userCreated = await fixture.Client.PostAsJsonAsync("/users", new { name = "John", email = "john@email.com" });
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>();
            var newEmail = "john.doe@email.com";

            //Act
            var emailChanged = await fixture.Client.PostAsJsonAsync("/users/change-email", new { id = user!.Id, newemail = newEmail });

            //Assert
            await fixture.Factory.FlushMetricsAsync();
            var metrics = fixture.Factory.Telemetry.Metrics;
            var emailChangedMetric = metrics.FirstOrDefault(m => m.Name == ChangeEmailMetrics.UsersEmailChangedCounterName);

            Assert.NotNull(emailChangedMetric);
            Assert.InRange(emailChangedMetric.GetCounterValue(), 1, 2);
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
