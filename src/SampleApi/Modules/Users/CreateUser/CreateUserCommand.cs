using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    /// <summary>
    /// Command: Create a new user.
    /// 
    /// Architecture Role:
    /// - Data Transfer Object (DTO)
    /// - Carries input data from PRIMARY ADAPTERS to INPUT PORTS
    /// - Immutable record type (no behavior)
    /// 
    /// Characteristics:
    /// 1. Immutable (record type)
    /// 2. Simple data container (no logic)
    /// 3. Validation happens in domain, not here
    /// 4. Serializable (for API endpoints)
    /// </summary>
    public sealed record CreateUserCommand(string Name, string Email) : ICommand;
}
