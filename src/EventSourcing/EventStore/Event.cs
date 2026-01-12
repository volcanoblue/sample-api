namespace VolcanoBlue.EventSourcing.EventStore
{
    /// <summary>
    /// [INFRASTRUCTURE - PERSISTENCE MODEL] Internal record for storing events in database.
    /// Architectural Role: Represents the physical storage structure of events in the event store.
    /// Maps domain events to database rows with serialized data.
    /// Id: Auto-incrementing sequence number for ordering.
    /// StreamId: Identifies which aggregate the event belongs to.
    /// OccurredAt: Timestamp when event was persisted (UTC).
    /// EventType: Fully qualified type name for deserialization.
    /// Data: JSON-serialized event data.
    /// Internal visibility prevents direct access from outside EventSourcing infrastructure.
    /// </summary>
    internal record Event(long Id, string StreamId, string OccurredAt, string EventType, string Data);
}
