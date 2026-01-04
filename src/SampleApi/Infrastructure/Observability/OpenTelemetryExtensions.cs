using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace VolcanoBlue.SampleApi.Infrastructure.Observability
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection AddObservability(this IServiceCollection services)
        {
            var serviceName = "VolcanoBlue.SampleApi";

            services.AddOpenTelemetry()
                .ConfigureResource(r => r.AddService(serviceName))
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddOtlpExporter();
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation()
                           .AddHttpClientInstrumentation()
                           .AddOtlpExporter();
                })
                .WithLogging(logging =>
                {
                    logging.AddOtlpExporter();
                });

            return services;
        }
    }
}
