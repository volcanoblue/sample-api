namespace VolcanoBlue.Core.Modeling
{
    /// <summary>
    /// [CORE - DDD MARKER] Marks a class as an Aggregate Root in DDD.
    /// Architectural Role: Identifies entities that define consistency boundaries
    /// and are the only entry points for modifications within their aggregate.
    /// </summary>
    public interface IAggregateRoot<T> where T : notnull
    {
        T Id { get; }
    }
}
