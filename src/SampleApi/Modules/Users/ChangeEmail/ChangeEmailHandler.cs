using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    public sealed class ChangeEmailHandler(IUserRepository repository, IUserViewStore store)
        : ICommandHandler<ChangeEmailCommand, Unit, IError>
    {
        public async Task<Result<Unit, IError>> HandleAsync(ChangeEmailCommand command, CancellationToken ct)
        {
            var userFound = repository.GetByIdAsync(command.Id, ct);
            if(userFound.IsError)
                return Result<Unit, IError>.Error(userFound.ErrorValue);

            var userOption = userFound.ResultValue;
            if(userOption.IsNone)
                return UserErrors.UserNotFound;

            var emailChanged = userOption.Get().ChangeEmail(command.NewEmail);
            if(emailChanged.IsError)
                return emailChanged;

            var userSaved = await repository.SaveAsync(userFound.ResultValue, ct);
            if(userSaved.IsError)
                return userSaved;

            var userView = UserViewMapper.FromDomain(userFound.ResultValue);
            return await store.StoreAsync(userView, ct);
        }
    }
}
