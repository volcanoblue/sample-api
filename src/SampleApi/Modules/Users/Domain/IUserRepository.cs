using Moonad;
using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public interface IUserRepository
    {
        Result<Option<User>, IError> GetByIdAsync(Guid id, CancellationToken ct);

        Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct);
    }
}
