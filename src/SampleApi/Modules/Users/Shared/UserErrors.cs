using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Shared
{
    /// <summary>
    /// Domain errors for User aggregate.
    /// 
    /// Architecture Role:
    /// - Part of ubiquitous language (domain-driven design)
    /// - Type-safe error representation
    /// - Used in Result types for error handling
    /// 
    /// Benefits:
    /// 1. Compile-time error checking
    /// 2. Explicit error cases (no magic strings)
    /// 3. Pattern matching support
    /// 4. Self-documenting code
    /// 
    /// Usage:
    /// if (name is null) return UserErrors.EmptyName;
    /// </summary>
    public static class UserErrors
    {
        public static readonly EmptyNameError EmptyName;
        public static readonly EmptyEmailError EmptyEmail;
        public static readonly UserNotFoundError UserNotFound;
    }

    // Specific error types
    public readonly struct EmptyNameError : IError { }
    public readonly struct EmptyEmailError : IError { }
    public readonly struct UserNotFoundError : IError { }
}
