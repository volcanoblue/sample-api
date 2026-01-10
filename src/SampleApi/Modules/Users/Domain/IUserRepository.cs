using Moonad;
using VolcanoBlue.Core.Error;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public interface IUserRepository
    {
        Task<Result<User, IError>> GetByIdAsync(Guid id, CancellationToken ct);

        Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct);
    }
}
