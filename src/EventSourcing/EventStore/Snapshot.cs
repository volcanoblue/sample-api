namespace VolcanoBlue.EventSourcing.EventStore
{
    internal record Snapshot(string EntityId, long EntityVersion, string Created, string Data);
}
