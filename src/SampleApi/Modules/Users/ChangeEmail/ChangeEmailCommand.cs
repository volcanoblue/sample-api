using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// Command: Change user's email address.
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
    /// 
    /// </summary>
    /// <param name="Id">Unique identifier of the user whose email will be changed</param>
    /// <param name="NewEmail">The new email address to be assigned</param>
    public sealed record ChangeEmailCommand(Guid Id, string NewEmail) : ICommand;
}
