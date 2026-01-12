namespace VolcanoBlue.EventSourcing.Abstractions
{
    /// <summary>
    /// [INFRASTRUCTURE - SNAPSHOT CONTRACT] Interface for entity snapshots in Event Sourcing.
    /// Architectural Role: Defines contract for snapshot optimization pattern.
    /// Snapshots store materialized state at a point in time to avoid replaying all events.
    /// EntityVersion indicates which event version the snapshot represents.
    /// Used to improve performance when reconstructing aggregates with long event histories.
    /// </summary>
    public interface ISnapshot
    {
        int EntityVersion { get; }
    }
}
