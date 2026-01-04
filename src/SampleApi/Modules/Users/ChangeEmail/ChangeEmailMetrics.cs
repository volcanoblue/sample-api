using System.Diagnostics.Metrics;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// Metrics for ChangeEmail use case observability.
    /// 
    /// Architecture Role:
    /// - Infrastructure concern (observability)
    /// - Used by PRIMARY ADAPTERS (endpoints)
    /// - Provides telemetry for monitoring and alerting
    /// 
    /// Integration:
    /// - Uses System.Diagnostics.Metrics API (.NET 8+)
    /// - Compatible with OpenTelemetry
    /// - Exported to monitoring systems (Prometheus, Application Insights, etc.)
    /// 
    /// Metrics Types:
    /// - Counter: Monotonically increasing value (counts events)
    /// - Gauge: Current value (active connections, queue size)
    /// - Histogram: Distribution of values (request duration)
    /// 
    /// Benefits:
    /// 1. Real-time monitoring
    /// 2. Alerting on anomalies
    /// 3. Performance analysis
    /// 4. Business metrics tracking
    /// </summary>
    public sealed class ChangeEmailMetrics(IMeterFactory meterFactory)
    {
        /// <summary>
        /// Meter name for the ChangeEmail use case.
        /// Used to group related metrics together.
        /// </summary>
        public const string UsersChangeEmailMetricName = "SampleApi.Users.ChangeEmail";
        
        /// <summary>
        /// Counter name for email changes.
        /// Convention: lowercase with underscores (Prometheus style).
        /// </summary>
        public const string UsersEmailChangedCounterName = "users_email_changed";

        /// <summary>
        /// Counter: Tracks the total number of successful email changes.
        /// 
        /// Usage:
        /// - Increment by 1 on each successful email change
        /// - Monitor rate of changes over time
        /// - Alert on unusual spikes or drops
        /// 
        /// Query Examples (Prometheus):
        /// - Rate: rate(users_email_changed[5m])
        /// - Total: sum(users_email_changed)
        /// </summary>
        public Counter<long> EmailChanged { get; } = meterFactory
            .Create(UsersChangeEmailMetricName)
            .CreateCounter<long>(UsersEmailChangedCounterName, 
                description: "Counts the number of times a user's email has been successfully changed.");
    }
}
