using System;
using System.Text.Json;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore
{
    internal sealed class SnapshotSerializer
    {
        public static Snapshot Serialize<T>(string snapshotId, T snapshot) where T : ISnapshot
        {
            return new(snapshotId,
                       snapshot.EntityVersion,
                       DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                       JsonSerializer.Serialize(snapshot));
        }

        public static T Deserialize<T>(Snapshot snapshot) =>
            JsonSerializer.Deserialize<T>(snapshot.Data)!;
    }
}
