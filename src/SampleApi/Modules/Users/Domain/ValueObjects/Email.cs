using Moonad;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Modeling;
using VolcanoBlue.SampleApi.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain.ValueObjects
{
    /// <summary>
    /// [DOMAIN - VALUE OBJECT] Represents an email address with validation enforced through factory method.
    /// Architectural Role: Domain primitive that encapsulates email business rules, ensuring invalid emails cannot exist in the domain model.
    /// </summary>
    public sealed class Email(string value) : ValueObject<Email, string>(value)
    {
        public static Result<Email, IError> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || EmailValidator.IsInvalid(value))
                return InvalidEmailError.Instance;

            return new Email(value);
        }
    }

    public sealed class InvalidEmailError : IError
    {
        private InvalidEmailError() { }

        public static readonly InvalidEmailError Instance = new();
    }
}
