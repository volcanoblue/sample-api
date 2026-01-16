using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore.Serialization
{
    /// <summary>
    /// [INFRASTRUCTURE - SERIALIZATION] Serializes and deserializes domain events for persistence.
    /// Architectural Role: Converts between domain event objects and storage format.
    /// </summary>
    internal static class EventSerializer
    {
        private static readonly Dictionary<string, Type> _typesCache = [];
        private static readonly JsonSerializerOptions options = new()
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            Converters = { new PrivateConstructorConverter() }
        };

        public static Event Serialize(string streamId, IEvent @event)
        {
            var type = @event.GetType();

            Event serialized = new(@event.EventId,
                                   streamId,
                                   DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                                   type.FullName!,
                                   JsonSerializer.Serialize(@event, type));

            return serialized;
        }

        public static IEvent Deserialize(Event @event)
        {
            var type = EventTypeRegistry.GetType(@event.EventType);
            var deserialized = (IEvent)JsonSerializer.Deserialize(@event.Data, type, options)!;

            return deserialized;
        }
    }
}
