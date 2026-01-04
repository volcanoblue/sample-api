using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using System.Diagnostics;
using VolcanoBlue.SampleApi.Tests.Api.Types;

namespace VolcanoBlue.SampleApi.Tests.Api.Fixture
{
    public sealed class InMemoryTelemetry
    {
        public BlockingList<Activity> Traces { get; } = [];
        public BlockingList<Metric> Metrics { get; } = [];
        public BlockingList<LogRecord> Logs { get; } = [];
    }
}
