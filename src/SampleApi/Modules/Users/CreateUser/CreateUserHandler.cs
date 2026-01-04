using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    /// <summary>
    /// USE CASE: Create a new user.
    /// 
    /// Architecture Role:
    /// - INPUT PORT IMPLEMENTATION: Executes the use case
    /// - APPLICATION LAYER: Orchestrates domain operations
    /// - Depends on OUTPUT PORTS (IUserRepository)
    /// - Invoked by PRIMARY ADAPTERS (REST API, CLI, etc.)
    /// 
    /// Responsibilities:
    /// 1. Receive command from adapter
    /// 2. Orchestrate domain operations (User.Create)
    /// 3. Call output ports for persistence
    /// 4. Return result to adapter
    /// 
    /// Does NOT contain:
    /// - HTTP concerns (status codes, routing)
    /// - Database concerns (SQL, connections)
    /// - Infrastructure concerns (logging, metrics)
    /// 
    /// Can be reused by different adapters:
    /// - REST API endpoint
    /// - gRPC service
    /// - CLI command
    /// - Message queue consumer
    /// </summary>
    public sealed class CreateUserHandler(IUserRepository repository) 
        : ICommandHandler<CreateUserCommand, User, IError>
    {
        /// <summary>
        /// Executes the create user use case.
        /// 
        /// Workflow:
        /// 1. Create user entity (domain validation)
        /// 2. If invalid, return error immediately (railway pattern)
        /// 3. Persist user through output port
        /// 4. If persistence fails, return error
        /// 5. Return created user
        /// </summary>
        public async Task<Result<User, IError>> HandleAsync(CreateUserCommand command, CancellationToken ct)
        {
            // Step 1: Execute domain logic (validation + creation)
            var userCreated = User.Create(Guid.NewGuid(), command.Name, command.Email);
            
            // Step 2: Early return on validation error (railway pattern)
            if (userCreated.IsError)
                return userCreated;

            // Step 3: Persist through output port (repository)
            var userSaved = await repository.SaveAsync(userCreated, ct);
            
            // Step 4: Handle persistence errors
            if(userSaved.IsError)
                return Result<User, IError>.Error(userSaved.ErrorValue);

            // Step 5: Return successful result
            return userCreated;
        }
    }
}
