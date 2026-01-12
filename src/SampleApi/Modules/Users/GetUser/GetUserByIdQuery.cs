using VolcanoBlue.Core.Query;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    /// <summary>
    /// [APPLICATION - INPUT PORT] Query for fetching user by ID.
    /// Architectural Role: Represents query intention. Separates queries from commands (CQRS).
    /// Input port for read operations.
    /// </summary>
    public sealed record GetUserByIdQuery(Guid Id) : IQuery;
}