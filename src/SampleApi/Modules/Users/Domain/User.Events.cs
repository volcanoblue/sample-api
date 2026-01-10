using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    public sealed partial class User
    {
        public sealed record UserCreated(int EventId, Guid Id, string Name, string Email) : IEvent;
        public sealed record UserEmailChanged(int EventId, Guid Id, string NewEmail) : IEvent;
    }
}
