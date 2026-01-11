namespace VolcanoBlue.EventSourcing.EventStore
{
    internal record Event(long Id, string StreamId, string OccurredAt, string EventType, string Data);
}
