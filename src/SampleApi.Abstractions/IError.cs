namespace VolcanoBlue.SampleApi.Abstractions
{
    /// <summary>
    /// Marker interface for domain errors.
    /// 
    /// Architecture Role:
    /// - Represents business rule violations
    /// - Type-safe error handling without exceptions
    /// - Used in Result<TSuccess, TError> pattern
    /// 
    /// Error Types (examples):
    /// - EmptyNameError
    /// - InvalidEmailError
    /// - UserNotFoundError
    /// - InsufficientFundsError
    /// 
    /// Benefits:
    /// - Compile-time error handling
    /// - Explicit business rule violations
    /// - No performance overhead of exceptions
    /// - Pattern matching support
    /// </summary>
    public interface IError
    {
    }
}