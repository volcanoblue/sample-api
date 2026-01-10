namespace VolcanoBlue.EventSourcing.Abstractions
{
    public interface ISnapshot
    {
        int EntityVersion { get; }
    }
}
