using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore
{
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
