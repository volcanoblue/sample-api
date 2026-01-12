using System.Diagnostics.Metrics;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// [INFRASTRUCTURE - OBSERVABILITY] Metrics for monitoring the ChangeEmail use case.
    /// Architectural Role: Provides telemetry for observability using OpenTelemetry.
    /// Enables monitoring, alerting, and performance analysis in production.
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
