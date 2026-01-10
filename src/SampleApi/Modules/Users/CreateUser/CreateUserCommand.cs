using VolcanoBlue.Core.Command;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    public sealed record CreateUserCommand(string Name, string Email) : ICommand;
}
