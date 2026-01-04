using Moonad;
using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// OUTPUT PORT: Repository interface for user persistence.
    /// 
    /// Architecture Role:
    /// - OUTPUT PORT (Driven side) - Abstraction for data access
    /// - Defined in DOMAIN layer (core)
    /// - Implemented by SECONDARY ADAPTERS (infrastructure)
    /// - Dependency Inversion: Core depends on abstraction, not implementation
    /// 
    /// Benefits:
    /// 1. Domain layer stays pure (no DB dependencies)
    /// 2. Infrastructure details isolated (can swap implementations)
    /// 3. Testability: Easy to mock for unit tests
    /// 4. Multiple implementations: InMemory, SQL, NoSQL, Cache
    /// 
    /// Implementation Examples:
    /// - InMemoryUserRepository (testing)
    /// - SqlUserRepository (production)
    /// - MongoUserRepository (alternative)
    /// - CachedUserRepository (decorator)
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a user by unique identifier.
        /// </summary>
        /// <param name="id">User unique identifier</param>
        /// <returns>Option containing User if found, None if not found</returns>
        Option<User> GetById(Guid id);

        /// <summary>
        /// Persists a user (create or update).
        /// </summary>
        /// <param name="user">User entity to persist</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Result with Unit on success, IError on failure</returns>
        Task<Result<Unit, IError>> SaveAsync(User user, CancellationToken ct);
    }
}
