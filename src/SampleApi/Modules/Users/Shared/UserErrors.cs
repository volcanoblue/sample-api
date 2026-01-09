using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Shared
{
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
