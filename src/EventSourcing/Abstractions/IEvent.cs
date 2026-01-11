namespace VolcanoBlue.EventSourcing.Abstractions
{
    public interface IEvent
    {
        long EventId { get; }
    }
}
