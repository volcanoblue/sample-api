using Moonad;
using System.Collections.Concurrent;
using VolcanoBlue.Core.Error;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Domain.Errors;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence
{
    /// <summary>
    /// [INFRASTRUCTURE - SECONDARY ADAPTER] In-memory storage for read model.
    /// Architectural Role: Secondary adapter for read projections (CQRS).
    /// Uses ConcurrentDictionary for thread-safe in-memory storage.
    /// Replace with read-optimized database in production.
    /// </summary>
    public sealed class InMemoryUserViewStorage(ILogger<InMemoryUserViewStorage> logger) : IUserViewStore
    {
        private readonly ConcurrentDictionary<Guid, UserView> _users = [];

        public async Task<Result<UserView, IError>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = _users.Values.FirstOrDefault(user => user.Id == id).ToOption();
            if(user)
                return await Task.FromResult(user.Get());

            return await Task.FromResult(UserViewNotFoundError.Instance);
        }

        public async Task<Result<Unit, IError>> StoreAsync(UserView userView, CancellationToken ct)
        {
            _users.AddOrUpdate(userView.Id, userView, (key, oldValue) => userView);

            logger.LogInformation("User with ID {UserId} saved to in-memory repository.", userView.Id);

            return await Task.FromResult(Unit.Value);
        }
    }
}
