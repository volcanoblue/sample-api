using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VolcanoBlue.EventSourcing.EventStore.Serialization
{
    /// <summary>
    /// [INFRASTRUCTURE - SERIALIZATION] Universal JSON converter for types with private constructors.
    /// Architectural Role: Enables deserialization of domain value objects by invoking private constructors via reflection.
    /// Automatically handles any type with private constructors regardless of parameter count.
    /// Preserves domain model purity by keeping serialization concerns in infrastructure layer.
    /// </summary>
    internal class PrivateConstructorConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            // Check if type has any non-public constructor
            var hasPrivateConstructor = typeToConvert
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .Length != 0;

            // Also check if all public constructors are parameterless (to avoid interfering with records/classes that already work)
            var hasOnlyParameterlessPublicConstructor = typeToConvert
                .GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .All(c => c.GetParameters().Length == 0);

            return hasPrivateConstructor && hasOnlyParameterlessPublicConstructor;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(PrivateConstructorConverterInner<>).MakeGenericType(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }

        private class PrivateConstructorConverterInner<T> : JsonConverter<T>
        {
            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                    return default;

                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                // Find the best matching constructor (prefer one with most parameters)
                var constructor = typeof(T)
                    .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .FirstOrDefault();

                if (constructor == null)
                    return default;

                var parameters = constructor.GetParameters();
                var parameterValues = new object?[parameters.Length];

                // Match JSON properties to constructor parameters
                for (int i = 0; i < parameters.Length; i++)
                {
                    var param = parameters[i];
                    var paramName = param.Name!;
                    
                    // Try to find matching property in JSON (case-insensitive)
                    if (root.TryGetProperty(paramName, out var jsonValue) ||
                        root.TryGetProperty(char.ToUpperInvariant(paramName[0]) + paramName.Substring(1), out jsonValue))
                    {
                        parameterValues[i] = JsonSerializer.Deserialize(jsonValue.GetRawText(), param.ParameterType, options);
                    }
                    else
                    {
                        // Use default value if property not found
                        parameterValues[i] = param.ParameterType.IsValueType 
                            ? Activator.CreateInstance(param.ParameterType) 
                            : null;
                    }
                }

                // Invoke constructor with matched parameter values
                return (T?)constructor.Invoke(parameterValues);
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}