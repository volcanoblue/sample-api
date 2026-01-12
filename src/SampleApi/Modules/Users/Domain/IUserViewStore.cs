using Moonad;
using VolcanoBlue.Core.Error;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// [DOMAIN - OUTPUT PORT] Interface for storing and querying read projections.
    /// Architectural Role: Output port for read model in CQRS pattern.
    /// Separates query responsibility from event persistence (Event Store).
    /// </summary>
    public interface IUserViewStore
    {
        Task<Result<Unit, IError>> StoreAsync(UserView userView, CancellationToken ct);
        Task<Result<UserView, IError>> GetByIdAsync(Guid id, CancellationToken ct);
    }
}
