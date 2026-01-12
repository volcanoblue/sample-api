namespace VolcanoBlue.Core.Modeling
{
    /// <summary>
    /// [CORE - BASE CLASS] Abstract base class for domain value objects providing equality by value, immutability, and primitive wrapping.
    /// Architectural Role: Core modeling abstraction that enforces value object semantics for domain primitives following DDD principles.
    /// </summary>
    public abstract class ValueObject<T, U>
        where T : ValueObject<T, U>
        where U : notnull
    {
        public U Value { get; }

        protected ValueObject(U value) =>
            Value = value;

        public static implicit operator U(ValueObject<T, U> valueObject) 
            => valueObject.Value;

        override public bool Equals(object? obj) =>
            obj is ValueObject<T, U> other && Value.Equals(other.Value);

        override public int GetHashCode() =>
            Value.GetHashCode();
    }
}
