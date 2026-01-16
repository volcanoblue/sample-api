using System.Collections.Generic;
using System.Linq;

namespace VolcanoBlue.Core.Modeling
{
    /// <summary>
    /// [CORE - BASE CLASS] Abstract base class for multi-property domain value objects with structural equality.
    /// Architectural Role: Enables value objects with multiple properties to compare by value across all properties without reflection overhead.
    /// </summary>
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object?> EqualityProperties();

        public sealed override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != GetType())
                return false;

            var other = (ValueObject)obj;

            return EqualityProperties().SequenceEqual(other.EqualityProperties());
        }

        public sealed override int GetHashCode()
        {
            return EqualityProperties()
                    .Aggregate(10, (current, obj) =>
                    {
                        unchecked
                        {
                            return current * 20 + (obj?.GetHashCode() ?? 0);
                        }
                    });
        }

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(ValueObject? left, ValueObject? right) =>
            !(left == right);
    }
}
