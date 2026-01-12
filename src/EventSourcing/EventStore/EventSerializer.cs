using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore
{
    /// <summary>
    /// [INFRASTRUCTURE - SERIALIZATION] Serializes and deserializes domain events for persistence.
    /// Architectural Role: Converts between domain event objects and storage format.
    /// Uses System.Text.Json for JSON serialization with type information preservation.
    /// Maintains type cache to optimize repeated deserialization of same event types.
    /// Serialization: Converts IEvent to Event record with JSON data and type metadata.
    /// Deserialization: Reconstructs strongly-typed domain events from stored Event records.
    /// Type resolution uses reflection to locate event types across all loaded assemblies.
    /// Internal visibility encapsulates serialization details from domain layer.
    /// </summary>
    internal sealed class EventSerializer
    {
        private static readonly Dictionary<string, Type> _types = [];

        public static Event Serialize(string streamId, IEvent @event)
        {
            var type = @event.GetType();

            return new(@event.EventId,
                       streamId,
                       DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                       type.FullName!,
                       JsonSerializer.Serialize(@event, type));
        }

        public static IEvent Desserialize(Event @event) =>
            (IEvent)JsonSerializer.Deserialize(@event.Data, GetType(@event.EventType))!;

        private static Type GetType(string typeName)
        {
            if (_types.TryGetValue(typeName, out var cached))
                return cached;

            var type = AppDomain.CurrentDomain
                                .GetAssemblies()
                                .SelectMany(a => a.GetTypes().Where(x => x.FullName == typeName))
                                .First();
            
            _types.Add(typeName, type);
            return type;
        }
    }
}
