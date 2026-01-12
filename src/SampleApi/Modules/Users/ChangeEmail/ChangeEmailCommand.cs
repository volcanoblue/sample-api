using System.ComponentModel.DataAnnotations;
using VolcanoBlue.Core.Command;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    public sealed record ChangeEmailCommand([Required] Guid Id, [EmailAddress]string NewEmail) : ICommand;
}
