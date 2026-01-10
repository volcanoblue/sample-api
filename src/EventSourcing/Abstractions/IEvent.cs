namespace VolcanoBlue.EventSourcing.Abstractions
{
    public interface IEvent
    {
        int EventId { get; }
    }
}
