using System.ComponentModel.DataAnnotations;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.DataAnnotations;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    public sealed record ChangeEmailCommand([Required] Guid Id, [Required][Email]string NewEmail) : ICommand;
}
