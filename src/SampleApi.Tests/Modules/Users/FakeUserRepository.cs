using Moonad;
using System.Collections.Concurrent;
using VolcanoBlue.Core.Error;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public sealed class FakeUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = [];

        public List<User> GetAll()
        {
            return [.. _users.Values];
        }

        public async Task<Result<User, IError>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            if (_users.TryGetValue(id, out User? user))
                return await Task.FromResult(user);
                
            return await Task.FromResult(UserNotFoundError.Instance);
        }

        public async Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct)
        {
            _users.AddOrUpdate(user.Id, user, (key, oldValue) => user);

            return await Task.FromResult(Unit.Value);
        }
    }
}
