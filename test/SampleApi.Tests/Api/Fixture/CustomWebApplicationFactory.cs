using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Tests.Modules.Users;

namespace VolcanoBlue.SampleApi.Tests.Api.Fixture
{
    public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private MeterProvider? _meterProvider;

        public FakeUserViewStore UserViewStore { get; } = new();
        public InMemoryTelemetry Telemetry { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IUserViewStore>();
                services.AddSingleton<IUserViewStore>(UserViewStore);
                services.AddSingleton<ChangeEmailMetrics>();
                
                var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IHostedService));
                if (descriptor is not null)
                    services.Remove(descriptor);

                services.AddOpenTelemetry()
                        .ConfigureResource(resource =>
                        {
                            resource.AddService("SampleApiTests");
                        })
                        .WithTracing(tpb =>
                        {
                            tpb.AddAspNetCoreInstrumentation()
                               .AddInMemoryExporter(Telemetry.Traces);
                        })
                        .WithMetrics(mpb =>
                        {
                            mpb.AddMeter(ChangeEmailMetrics.UsersChangeEmailMetricName)
                               .AddInMemoryExporter(Telemetry.Metrics);
                        });

                services.AddLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddOpenTelemetry(options =>
                    {
                        options.IncludeScopes = true;
                        options.IncludeFormattedMessage = true;
                        options.ParseStateValues = true;
                        options.AddInMemoryExporter(Telemetry.Logs);
                    });
                });
            });

            base.ConfigureWebHost(builder);
        }

        public async Task FlushMetricsAsync()
        {
            _meterProvider ??= Services.GetRequiredService<MeterProvider>();
            _meterProvider.ForceFlush();

            await Task.CompletedTask;
        }
    }
}