using Moonad;
using VolcanoBlue.Core.Error;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public interface IUserViewStore
    {
        Task<Result<Unit, IError>> StoreAsync(UserView userView, CancellationToken ct);
        Task<Result<UserView, IError>> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
