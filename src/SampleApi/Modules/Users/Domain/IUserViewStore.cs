using Moonad;
using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public interface IUserViewStore
    {
        Task<Result<Unit, IError>> StoreAsync(UserView userView, CancellationToken ct);
        Task<Result<Option<UserView>, IError>> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
