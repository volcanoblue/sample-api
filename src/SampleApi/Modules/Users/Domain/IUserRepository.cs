using Moonad;
using VolcanoBlue.Core.Error;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// [DOMAIN - OUTPUT PORT] Interface defining contract for user persistence.
    /// Architectural Role: Output port in Hexagonal Architecture, abstracts persistence details.
    /// Allows domain to not depend on specific infrastructure technologies.
    /// </summary>
    public interface IUserRepository
    {
        Task<Result<User, IError>> GetByIdAsync(Guid id, CancellationToken ct);

        Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct);
    }
}
