using Moonad;
using System.Collections.Concurrent;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence
{
    public sealed class InMemoryUserViewStorage(ILogger<InMemoryUserViewStorage> logger) : IUserViewStore
    {
        private readonly ConcurrentDictionary<Guid, UserView> _users = [];

        public async Task<Result<Option<UserView>, IError>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var user = _users.Values.FirstOrDefault(user => user.Id == id).ToOption();

            var result = user
                        ? Result<Option<UserView>, IError>.Ok(user)
                        : Result<Option<UserView>, IError>.Error(UserErrors.UserNotFound);

            return await Task.FromResult(result);
        }

        public async Task<Result<Unit, IError>> StoreAsync(UserView userView, CancellationToken ct)
        {
            _users.AddOrUpdate(userView.Id, userView, (key, oldValue) => userView);

            logger.LogInformation("User with ID {UserId} saved to in-memory repository.", userView.Id);

            return await Task.FromResult(Unit.Value);
        }
    }
}
