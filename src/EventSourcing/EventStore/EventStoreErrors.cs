using VolcanoBlue.Core.Error;

namespace VolcanoBlue.EventSourcing.EventStore
{
    public sealed class SnapshotNotFoundError : IError
    {   
        private SnapshotNotFoundError() { }
        public static SnapshotNotFoundError Instance { get; } = new SnapshotNotFoundError();
    }
}
