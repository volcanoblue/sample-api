using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public sealed partial class User
    {
        public sealed record UserCreated(long EventId, Guid Id, string Name, string Email) : IEvent;
        public sealed record UserEmailChanged(long EventId, Guid Id, string NewEmail) : IEvent;
    }
}
