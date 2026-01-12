namespace VolcanoBlue.EventSourcing.Abstractions
{
    /// <summary>
    /// [DOMAIN - EVENT CONTRACT] Base interface for all domain events in Event Sourcing.
    /// Architectural Role: Defines the contract that all domain events must implement.
    /// EventId represents the sequence number in the event stream and serves as the entity version.
    /// Events are immutable records of facts that have occurred in the domain.
    /// </summary>
    public interface IEvent
    {
        long EventId { get; }
    }
}
