using VolcanoBlue.EventSourcing.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain.ValueObjects;

namespace VolcanoBlue.SampleApi.Modules.Users.Domain
{
    /// <summary>
    /// [DOMAIN - DOMAIN EVENTS] Domain events for the User entity.
    /// Architectural Role: Defines immutable events representing facts that occurred in the domain.
    /// Part of Event Sourcing implementation where each state change is recorded as an event.
    /// </summary>
    public sealed partial class User
    {
        public sealed record UserCreated(long EventId, Guid Id, Name Name, Email Email) : IEvent;
        public sealed record UserEmailChanged(long EventId, Guid Id, Email NewEmail) : IEvent;
    }
}
