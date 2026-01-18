using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VolcanoBlue.EventSourcing.Abstractions;

namespace VolcanoBlue.EventSourcing.EventStore.Serialization
{
    /// <summary>
    /// [INFRASTRUCTURE - TYPE REGISTRY] Cached registry for event type mappings.
    /// Architectural Role: Scans assemblies once at startup to build type cache.
    /// Provides fast O(1) type lookup during event deserialization.
    /// </summary>
    public static class EventTypeRegistry
    {
        private static readonly ConcurrentDictionary<string, Type> _types = [];
        private static bool _isInitialized = false;

        public static void Initialize(params Assembly[] assemblies)
        {
            if (_isInitialized)
                return;

            var eventTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in eventTypes)
            {
                if (!_types.ContainsKey(type.FullName!))
                {
                    _types[type.FullName!] = type;
                }
            }

            _isInitialized = true;
        }

        public static Type GetType(string typeName)
        {
            if (_types.TryGetValue(typeName, out var type))
                return type;

            throw new InvalidOperationException(
                $"Event type '{typeName}' not found. Ensure EventTypeRegistry.Initialize() is called with correct assemblies.");
        }
    }
}