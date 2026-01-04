using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence
{
    /// <summary>
    /// SECONDARY ADAPTER: In-memory repository implementation.
    /// 
    /// Architecture Role:
    /// - SECONDARY ADAPTER (Driven side) - Implements OUTPUT PORT
    /// - Provides data persistence capabilities
    /// - Infrastructure concern (outside core domain)
    /// 
    /// Implementation Details:
    /// - In-memory storage (List<User>)
    /// - Suitable for: Testing, prototyping, development
    /// - NOT for production (data lost on restart)
    /// 
    /// Production Alternatives:
    /// - SqlUserRepository (Entity Framework Core)
    /// - MongoUserRepository (MongoDB driver)
    /// - CosmosUserRepository (Azure Cosmos DB)
    /// 
    /// Hexagonal Architecture Benefit:
    /// Swap implementations without changing domain or use cases.
    /// Register different implementations in DI container.
    /// </summary>
    public sealed class InMemoryUserRepository(ILogger<InMemoryUserRepository> logger) : IUserRepository
    {
        // In-memory storage (thread-safe alternatives: ConcurrentDictionary, ConcurrentBag)
        private readonly List<User> _users = [];

        /// <summary>
        /// Retrieves a user by ID from in-memory storage.
        /// </summary>
        public Option<User> GetById(Guid id)
        {
            // Find user and convert to Option type
            // Option.Some if found, Option.None if not found
            return _users.FirstOrDefault(user => user.Id == id).ToOption();
        }

        /// <summary>
        /// Saves a user to in-memory storage (create or update).
        /// </summary>
        public async Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct)
        {
            // Handle cancellation
            if (ct.IsCancellationRequested)
                return await Task.FromResult(CancellationTokenErrors.CancellationRequested);

            // Update existing or add new
            if (_users.Contains(user))
                _users[_users.IndexOf(user)] = user;
            else
                _users.Add(user);

            // Infrastructure concern: Logging (not in domain)
            logger.LogInformation("User with ID {UserId} saved to in-memory repository.", user.Id);

            return await Task.FromResult(Unit.Value);
        }
    }
}
