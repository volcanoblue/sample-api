namespace VolcanoBlue.EventSourcing.EventStore
{
    internal record Event(int Id, string StreamId, string Created, string EventType, string Data);
}
