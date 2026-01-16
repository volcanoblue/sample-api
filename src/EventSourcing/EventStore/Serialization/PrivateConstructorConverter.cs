using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VolcanoBlue.EventSourcing.EventStore.Serialization
{
    /// <summary>
    /// [INFRASTRUCTURE - SERIALIZATION] Universal JSON converter for types with private constructors.
    /// Architectural Role: Enables deserialization of domain value objects by invoking private constructors via reflection.
    /// Preserves domain model purity by keeping serialization concerns in infrastructure layer.
    /// </summary>
    internal sealed class PrivateConstructorConverter : JsonConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, bool> _canConvertCache = [];

        public override bool CanConvert(Type typeToConvert)
        {
            return _canConvertCache.GetOrAdd(typeToConvert, static type =>
            {
                var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                var hasPrivateConstructor = constructors.Any(c => !c.IsPublic);
                if (!hasPrivateConstructor)
                    return false;

                var publicConstructors = constructors.Where(c => c.IsPublic);
                var hasOnlyParameterlessPublicConstructor = !publicConstructors.Any() ||
                                                            publicConstructors.All(c => c.GetParameters().Length == 0);

                return hasOnlyParameterlessPublicConstructor;
            });
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(PrivateConstructorConverterInner<>).MakeGenericType(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }

        private sealed class PrivateConstructorConverterInner<T> : JsonConverter<T>
        {
            private static readonly ConstructorInfo? _constructor;
            private static readonly ParameterInfo[]? _parameters;
            private static readonly PropertyNameMapping[]? _propertyMappings;
            private static readonly Func<object?[], T>? _compiledConstructor;
            private static readonly int _parameterCount;

            static PrivateConstructorConverterInner()
            {
                // Cache constructor and parameter metadata at type initialization
                _constructor = typeof(T)
                    .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .FirstOrDefault();

                if (_constructor is not null)
                {
                    _parameters = _constructor.GetParameters();
                    _parameterCount = _parameters.Length;
                    _propertyMappings = new PropertyNameMapping[_parameterCount];

                    for (int i = 0; i < _parameterCount; i++)
                    {
                        var paramName = _parameters[i].Name!;
                        _propertyMappings[i] = new PropertyNameMapping(
                            paramName,
                            paramName.Length > 0 ? char.ToUpperInvariant(paramName[0]) + paramName[1..] : paramName
                        );
                    }

                    // Compile expression tree for fast constructor invocation
                    _compiledConstructor = BuildConstructorDelegate();
                }
            }

            private static Func<object?[], T> BuildConstructorDelegate()
            {
                if (_constructor is null || _parameters is null)
                    throw new InvalidOperationException("Constructor not found");

                // Handle parameterless constructor
                if (_parameters.Length == 0)
                {
                    var constructorCall = Expression.New(_constructor);
                    var lambda = Expression.Lambda<Func<object?[], T>>(
                        constructorCall,
                        Expression.Parameter(typeof(object?[]), "args")
                    );
                    return lambda.Compile();
                }

                // Create parameter: object?[] args
                var argsParam = Expression.Parameter(typeof(object?[]), "args");

                // Create expressions: (TParam)args[i] for each parameter
                var parameterExpressions = new Expression[_parameters.Length];
                for (int i = 0; i < _parameters.Length; i++)
                {
                    var paramType = _parameters[i].ParameterType;
                    var arrayAccess = Expression.ArrayIndex(argsParam, Expression.Constant(i));

                    // Handle nullable value types and reference types
                    Expression convertExpression;
                    if (paramType.IsValueType && Nullable.GetUnderlyingType(paramType) == null)
                    {
                        // Non-nullable value type: handle null by providing default value
                        var nullCheck = Expression.Equal(arrayAccess, Expression.Constant(null, typeof(object)));
                        var defaultValue = Expression.Default(paramType);
                        var convertedValue = Expression.Convert(arrayAccess, paramType);
                        convertExpression = Expression.Condition(nullCheck, defaultValue, convertedValue);
                    }
                    else
                    {
                        // Nullable value types or reference types: direct conversion
                        convertExpression = Expression.Convert(arrayAccess, paramType);
                    }

                    parameterExpressions[i] = convertExpression;
                }

                // Create: new T((TParam1)args[0], (TParam2)args[1], ...)
                var constructorCall2 = Expression.New(_constructor, parameterExpressions);

                // Compile: object?[] => new T(...)
                var lambda2 = Expression.Lambda<Func<object?[], T>>(constructorCall2, argsParam);
                return lambda2.Compile();
            }

            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                    return default;

                if (_constructor is null || _parameters is null || _propertyMappings is null || _compiledConstructor is null)
                    return default;

                // Handle parameterless constructor
                if (_parameterCount == 0)
                {
                    return _compiledConstructor([]);
                }

                using var doc = JsonDocument.ParseValue(ref reader);
                var root = doc.RootElement;

                // Rent array from pool for parameter values - handle any size
                var parameterValues = _parameterCount <= 256
                    ? ArrayPool<object?>.Shared.Rent(_parameterCount)
                    : new object?[_parameterCount]; // Fallback for extremely large parameter lists

                try
                {
                    // Match JSON properties to constructor parameters
                    for (int i = 0; i < _parameterCount; i++)
                    {
                        var param = _parameters[i];
                        var mapping = _propertyMappings[i];

                        // Try to find matching property in JSON (case-insensitive)
                        if (root.TryGetProperty(mapping.LowerCase, out var jsonValue) ||
                            root.TryGetProperty(mapping.UpperCase, out jsonValue))
                        {
                            parameterValues[i] = JsonSerializer.Deserialize(jsonValue.GetRawText(), param.ParameterType, options);
                        }
                        else
                        {
                            // Use default value if property not found
                            parameterValues[i] = param.HasDefaultValue
                                ? param.DefaultValue
                                : GetDefaultValue(param.ParameterType);
                        }
                    }

                    // Invoke compiled constructor delegate
                    return _compiledConstructor(parameterValues);
                }
                finally
                {
                    // Return array to pool only if it was rented
                    if (_parameterCount <= 256)
                    {
                        ArrayPool<object?>.Shared.Return(parameterValues, clearArray: true);
                    }
                }
            }

            private static object? GetDefaultValue(Type type)
            {
                // Handle nullable value types
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null)
                    return null;

                // Handle value types
                if (type.IsValueType)
                    return Activator.CreateInstance(type);

                // Reference types default to null
                return null;
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, value, value!.GetType(), options);
            }

            private readonly record struct PropertyNameMapping(string LowerCase, string UpperCase);
        }
    }
}