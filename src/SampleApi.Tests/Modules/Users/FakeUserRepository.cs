using Moonad;
using System.Collections.Concurrent;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public sealed class FakeUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = [];

        public List<User> GetAll()
        {
            return [.. _users.Values];
        }

        public Option<User> GetById(Guid id)
        {
            return _users.TryGetValue(id, out User? user)
                ? user.ToOption()
                : Option.None<User>();
        }

        public async Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct)
        {
            if (ct.IsCancellationRequested)
                return await Task.FromResult(CancellationTokenErrors.CancellationRequested);

            _users.AddOrUpdate(user.Id, user, (key, oldValue) => user);

            return await Task.FromResult(Unit.Value);
        }
    }
}
