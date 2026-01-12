using VolcanoBlue.Core.Command;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    /// <summary>
    /// [APPLICATION - INPUT PORT] Command for creating a new user.
    /// Architectural Role: Represents intention to create user. Input port in Hexagonal Architecture.
    /// Defines technology-agnostic input contract (HTTP, gRPC, CLI, etc).
    /// </summary>
    public sealed record CreateUserCommand(string Name, string Email) : ICommand;
}
