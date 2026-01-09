using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    public sealed class CreateUserHandler(IUserRepository repository, IUserViewStore storage)
        : ICommandHandler<CreateUserCommand, User, IError>
    {
        public async Task<Result<User, IError>> HandleAsync(CreateUserCommand command, CancellationToken ct)
        {
            var userCreated = User.Create(Guid.NewGuid(), command.Name, command.Email);
            if (userCreated.IsError)
                return userCreated;

            var userSaved = await repository.SaveAsync(userCreated, ct);
            if(userSaved.IsError)
                return Result<User, IError>.Error(userSaved.ErrorValue);

            var viewSaved = await storage.StoreAsync(UserViewMapper.FromDomain(userCreated.ResultValue), ct);
            if(viewSaved.IsError)
                return Result<User, IError>.Error(viewSaved.ErrorValue);

            return userCreated;
        }
    }
}
