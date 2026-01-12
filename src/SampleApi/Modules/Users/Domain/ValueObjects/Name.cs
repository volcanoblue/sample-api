using Moonad;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Modeling;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain.ValueObjects
{
    /// <summary>
    /// [DOMAIN - VALUE OBJECT] Represents a user's name with validation enforced through factory method.
    /// Architectural Role: Domain primitive that encapsulates name business rules, preventing null or empty names from existing in the domain model.
    /// </summary>
    public sealed class Name(string value) : ValueObject<Name, string>(value)
    {
        public static Result<Name, IError> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return InvalidNameError.Instance;

            return new Name(value);
        }
    }

    public sealed class InvalidNameError : IError
    {
        private InvalidNameError() { }

        public static readonly InvalidNameError Instance = new();
    }
}
