using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// USE CASE: Change user's email address.
    /// 
    /// Architecture Role:
    /// - INPUT PORT IMPLEMENTATION: Executes the use case
    /// - APPLICATION LAYER: Orchestrates domain operations
    /// - Depends on OUTPUT PORTS (IUserRepository)
    /// - Invoked by PRIMARY ADAPTERS (REST API, CLI, etc.)
    /// 
    /// Responsibilities:
    /// 1. Receive command from adapter
    /// 2. Retrieve user from repository (output port)
    /// 3. Execute domain operation (User.ChangeEmail)
    /// 4. Persist changes through repository
    /// 5. Return result to adapter
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
    public sealed class ChangeEmailHandler(IUserRepository repository) 
        : ICommandHandler<ChangeEmailCommand, Unit, IError>
    {
        /// <summary>
        /// Executes the change email use case.
        /// 
        /// Workflow:
        /// 1. Retrieve user by ID from repository
        /// 2. If not found, return UserNotFound error (railway pattern)
        /// 3. Execute domain operation: ChangeEmail (validation)
        /// 4. If validation fails, return error immediately
        /// 5. Persist updated user through output port
        /// 6. Return persistence result
        /// </summary>
        /// <param name="command">Command containing user ID and new email</param>
        /// <param name="ct">Cancellation token for operation cancellation</param>
        /// <returns>Result with Unit on success, IError on failure</returns>
        public async Task<Result<Unit, IError>> HandleAsync(ChangeEmailCommand command, CancellationToken ct)
        {
            // Step 1: Retrieve user from repository (output port)
            var userFound = repository.GetById(command.Id);
            
            // Step 2: Early return if user not found (railway pattern)
            if(userFound.IsNone)
                return UserErrors.UserNotFound;

            // Step 3: Execute domain logic (email validation and change)
            var emailChanged = userFound.Get().ChangeEmail(command.NewEmail);
            
            // Step 4: Handle validation errors
            if(emailChanged.IsError)
                return emailChanged;

            // Step 5: Persist updated user through output port
            var userSaved = await repository.SaveAsync(userFound, ct);
            
            // Step 6: Return persistence result
            return userSaved;
        }
    }
}
