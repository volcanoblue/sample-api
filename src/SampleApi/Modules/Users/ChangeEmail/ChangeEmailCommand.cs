using VolcanoBlue.Core.Command;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    public sealed record ChangeEmailCommand(Guid Id, string NewEmail) : ICommand;
}
