namespace VolcanoBlue.Core.Error
{
    /// <summary>
    /// [APPLICATION - MARKER INTERFACE] Base marker interface for all error types.
    /// Architectural Role: Identifies error objects in Railway-Oriented Programming pattern.
    /// Enables type-safe error handling without throwing exceptions for expected failures.
    /// Marker interface allows pattern matching on specific error types for appropriate responses.
    /// Used with Result type to make errors explicit in method signatures.
    /// </summary>
    public interface IError
    {
    }
}