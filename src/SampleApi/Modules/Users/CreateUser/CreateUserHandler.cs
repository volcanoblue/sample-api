using Moonad;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.Error;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    /// <summary>
    /// [APPLICATION - USE CASE] Handler that orchestrates the create user use case.
    /// Architectural Role: Implements application logic, coordinates domain entity and repository.
    /// Implements Railway-Oriented Programming with Result types for explicit error handling.
    /// </summary>
    public sealed class CreateUserHandler(IUserRepository userRepository, IUserViewStore userViewStorage)
        : ICommandHandler<CreateUserCommand, User, IError>
    {
        public async Task<Result<User, IError>> HandleAsync(CreateUserCommand command, CancellationToken ct)
        {
            var userCreated = User.Create(Guid.NewGuid(), command.Name, command.Email);
            if (userCreated.IsError)
                return userCreated;

            var userSaved = await userRepository.SaveAsync(userCreated, ct);
            if(userSaved.IsError)
                return Result<User, IError>.Error(userSaved.ErrorValue);

            // This is a dual-write scenario; in a real-world app we'd need to handle potential inconsistencies
            var viewSaved = await userViewStorage.StoreAsync(UserViewMapper.FromEntity(userCreated.ResultValue), ct);
            if(viewSaved.IsError)
                return Result<User, IError>.Error(viewSaved.ErrorValue);

            return userCreated;
        }
    }
}
