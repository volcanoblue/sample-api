using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    public sealed record CreateUserCommand(string Name, string Email) : ICommand;
}
