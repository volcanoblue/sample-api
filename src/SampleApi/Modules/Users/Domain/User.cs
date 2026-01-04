using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// DOMAIN ENTITY: User aggregate root.
    /// 
    /// Architecture Role:
    /// - CORE DOMAIN - Contains business logic and invariants
    /// - Aggregate root: Consistency boundary for user data
    /// - No dependencies on infrastructure or frameworks
    /// - Pure C# with business rules
    /// 
    /// Characteristics:
    /// 1. Encapsulation: Private setters, public methods for state changes
    /// 2. Immutability: Properties can only change through methods
    /// 3. Factory method: Static Create() ensures valid construction
    /// 4. Business rules: Validates all state transitions
    /// 5. Self-validation: Returns errors instead of throwing exceptions
    /// 
    /// Design Patterns:
    /// - Factory Method (Create)
    /// - Railway-Oriented Programming (Result type)
    /// - Defensive Programming (validation in methods)
    /// </summary>
    public sealed class User
    {
        // Properties with business meaning
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; private set; }

        // Private constructor: Force creation through factory method
        private User(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        /// <summary>
        /// Factory method: Creates a user with validation.
        /// 
        /// Business Rules:
        /// 1. Name cannot be null or empty
        /// 2. Email cannot be null or empty
        /// 
        /// Returns:
        /// - Success: Valid User instance
        /// - Error: Specific domain error (EmptyNameError, EmptyEmailError)
        /// </summary>
        public static Result<User, IError> Create(Guid id, string name, string email)
        {
            // Business rule: Name is required
            if(name is null or { Length: 0 })
                return UserErrors.EmptyName;

            // Business rule: Email is required
            if (email is null or { Length: 0 })
                return UserErrors.EmptyEmail;

            // All validations passed: create valid entity
            return new User(id, name, email);
        }

        /// <summary>
        /// Business operation: Changes user email with validation.
        /// 
        /// Business Rules:
        /// 1. New email cannot be null or empty
        /// 
        /// Returns:
        /// - Success: Unit (void equivalent)
        /// - Error: EmptyEmailError
        /// </summary>
        public Result<Unit, IError> ChangeEmail(string newEmail)
        {
            // Business rule: Email cannot be empty
            if (newEmail is null or { Length: 0 })
                return UserErrors.EmptyEmail;
            
            // Apply state change
            Email = newEmail;

            // Return success indicator
            return Unit.Value;
        }
    }
}
