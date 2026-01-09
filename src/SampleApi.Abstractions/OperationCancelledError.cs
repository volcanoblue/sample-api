namespace VolcanoBlue.SampleApi.Abstractions
{
    /// <summary>
    /// Represents an error when an operation is cancelled via CancellationToken.
    /// Used as a type-safe alternative to OperationCanceledException in Result types.
    /// </summary>
    public readonly struct OperationCancelledError : IError { }

    /// <summary>
    /// Provides static instances of cancellation-related errors.
    /// </summary>
    public static class CancellationTokenErrors
    {
        /// <summary>
        /// Singleton instance representing a cancelled operation.
        /// Return this when CancellationToken.IsCancellationRequested is true.
        /// </summary>
        public static readonly OperationCancelledError OperationCancelled;
    }
}
