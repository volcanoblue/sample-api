using VolcanoBlue.Core.Error;

namespace VolcanoBlue.SampleApi.Modules.Users.Shared
{
    public sealed class EmptyNameError : IError
    { 
        private EmptyNameError() { }

        public static readonly EmptyNameError Instance = new();
    }

    public sealed class EmptyEmailError : IError 
    { 
        private EmptyEmailError() { }

        public static readonly EmptyEmailError Instance = new();
    }

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
