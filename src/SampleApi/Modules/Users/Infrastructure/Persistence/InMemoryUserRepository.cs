using Moonad;
using System.Collections.Concurrent;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence
{
    public sealed class InMemoryUserRepository(ILogger<InMemoryUserRepository> logger) : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = [];

        public Result<Option<User>, IError> GetByIdAsync(Guid id, CancellationToken ct) =>
            _users.Values.FirstOrDefault(user => user.Id == id).ToOption();

        public async Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct)
        {
            _users.AddOrUpdate(user.Id, user, (key, oldValue) => user);

            logger.LogInformation("User with ID {UserId} saved to in-memory repository.", user.Id);

            return await Task.FromResult(Unit.Value);
        }
    }
}
