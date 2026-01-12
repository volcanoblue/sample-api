using System;

namespace VolcanoBlue.Core.Error
{
    /// <summary>
    /// [APPLICATION - ERROR TYPE] Wraps unexpected exceptions as IError for consistent error handling.
    /// Architectural Role: Bridges exception-based error handling with Railway-Oriented Programming.
    /// Used when infrastructure throws unexpected exceptions that need to be surfaced as Result errors.
    /// Allows catching exceptions in infrastructure layer and returning them as domain-friendly errors.
    /// Typically used for technical failures (database connection, network issues) vs business rule violations.
    /// </summary>
    public sealed class ExceptionalError : IError
    {
        public Exception Exception { get; }

        public ExceptionalError(Exception exception)
        {
            Exception = exception;
        }
    }
}
