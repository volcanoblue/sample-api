using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    public sealed record ChangeEmailCommand(Guid Id, string NewEmail) : ICommand;
}
