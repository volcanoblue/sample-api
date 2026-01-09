using Moonad;
using System.Collections.Concurrent;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public sealed class FakeUserViewStore : IUserViewStore
    {
        private readonly ConcurrentDictionary<Guid, UserView> _views = [];

        public async Task<Result<Option<UserView>, IError>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            if(! _views.TryGetValue(id, out UserView? userView))
                return UserErrors.UserNotFound;

            return await Task.FromResult(userView.ToOption());
        }

        public async Task<Result<Unit, IError>> StoreAsync(UserView userView, CancellationToken ct)
        {
            _views.AddOrUpdate(userView.Id, userView, (key, oldValue) => userView);

            return await Task.FromResult(Unit.Value);
        }
    }
}
