using Moonad;
using VolcanoBlue.Core.Error;
using VolcanoBlue.EventSourcing.EventStore;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence
{
    public sealed class InMemoryUserRepository(EventDbContext eventStore) : IUserRepository
    {
        private readonly string streamPrefix = "User-";

        public async Task<Result<User, IError>> GetByIdAsync(Guid id, CancellationToken ct)
        {
            var stream = await eventStore.ReadStreamAsync($"{streamPrefix}{id}", ct);
            return User.Restore(stream);
        }

        public async Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct)
        {
            await eventStore.AppendToStreamAsync($"{streamPrefix}{user.Id}", user.UncommittedEvents, ct);
            await eventStore.SaveChangesAsync(ct);
            
            return Unit.Value;
        }
    }
}
