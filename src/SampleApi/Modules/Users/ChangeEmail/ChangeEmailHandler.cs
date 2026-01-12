using Moonad;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.Error;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// [APPLICATION - USE CASE] Handler that orchestrates the change email use case.
    /// Architectural Role: Fetches user, applies change through domain method, and persists.
    /// Ensures business rules are enforced through domain entity.
    /// </summary>
    public sealed class ChangeEmailHandler(IUserRepository repository, IUserViewStore store)
        : ICommandHandler<ChangeEmailCommand, Unit, IError>
    {
        public async Task<Result<Unit, IError>> HandleAsync(ChangeEmailCommand command, CancellationToken ct)
        {
            var userFound = await repository.GetByIdAsync(command.Id, ct);
            if(userFound.IsError)
                return Result<Unit, IError>.Error(userFound.ErrorValue);

            var user = userFound.ResultValue;
            
            var emailChanged = user.ChangeEmail(command.NewEmail);
            if(emailChanged.IsError)
                return emailChanged;

            var userSaved = await repository.SaveAsync(userFound.ResultValue, ct);
            if(userSaved.IsError)
                return userSaved;

            var userView = UserViewMapper.FromEntity(userFound.ResultValue);
            return await store.StoreAsync(userView, ct);
        }
    }
}
