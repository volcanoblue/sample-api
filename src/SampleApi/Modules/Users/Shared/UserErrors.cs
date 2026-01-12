using VolcanoBlue.Core.Error;

namespace VolcanoBlue.SampleApi.Modules.Users.Shared
{
    /// <summary>
    /// [DOMAIN - ERROR TYPES] Domain-specific error types for users.
    /// Architectural Role: Represents business errors explicitly and type-safe.
    /// Implements Railway-Oriented Programming enabling structured failure handling.
    /// </summary>
    public sealed class UserNotFoundError : IError
    {
        private UserNotFoundError() { }

        public static readonly UserNotFoundError Instance = new();
    }

    public sealed class UserViewNotFoundError : IError
    {
        private UserViewNotFoundError() { }

        public static readonly UserViewNotFoundError Instance = new();
    }
}
