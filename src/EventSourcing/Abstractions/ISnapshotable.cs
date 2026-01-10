namespace VolcanoBlue.EventSourcing.Abstractions
{
    public interface ISnapshotable<T>
    {
        bool ShouldTakeSnapshot();

        T TakeSnapshot();
    }
}
