namespace VolcanoBlue.EventSourcing.EventStore
{
    /// <summary>
    /// [INFRASTRUCTURE - PERSISTENCE MODEL] Internal record for storing entity snapshots.
    /// Architectural Role: Represents the physical storage structure of snapshots in database.
    /// Snapshots are optimization technique to avoid replaying all events for long-lived aggregates.
    /// EntityId: Identifies which aggregate this snapshot belongs to.
    /// EntityVersion: Event version number at which this snapshot was taken.
    /// Created: Timestamp when snapshot was created (UTC).
    /// Data: JSON-serialized snapshot state.
    /// Internal visibility prevents direct access from outside EventSourcing infrastructure.
    /// Currently not actively used but provides foundation for snapshot optimization.
    /// </summary>
    internal record Snapshot(string EntityId, long EntityVersion, string Created, string Data);
}
