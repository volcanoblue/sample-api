using Moonad;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Modeling;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain.ValueObjects
{
    /// <summary>
    /// [DOMAIN - VALUE OBJECT] Represents a user's name with validation enforced through factory method.
    /// Architectural Role: Domain primitive that encapsulates name business rules, preventing null or empty names from existing in the domain model.
    /// </summary>
    public sealed class Name : ValueObject
    {
        public string FullName { get; private set; }

        private Name(string fullName) =>
            FullName = fullName;  

        public static Result<Name, IError> Create(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return InvalidNameError.Instance;

            return new Name(fullName);
        }

        public static implicit operator string(Name name) =>
            name.FullName;

        protected override IEnumerable<object?> EqualityProperties()
        {
            yield return FullName;
        }
    }

    public sealed class InvalidNameError : IError
    {
        private InvalidNameError() { }

        public static readonly InvalidNameError Instance = new();
    }
}
