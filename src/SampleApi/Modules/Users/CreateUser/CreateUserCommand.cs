using System.ComponentModel.DataAnnotations;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.DataAnnotations;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    /// <summary>
    /// [APPLICATION - INPUT PORT] Command for creating a new user.
    /// Architectural Role: Represents intention to create user. Input port in Hexagonal Architecture.
    /// Defines technology-agnostic input contract (HTTP, gRPC, CLI, etc).
    /// </summary>
    public sealed record CreateUserCommand([Required] string Name, [Required][Email] string Email) : ICommand;
}
