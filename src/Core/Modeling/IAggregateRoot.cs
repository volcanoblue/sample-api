using System;

namespace VolcanoBlue.Core.Modeling
{
    /// <summary>
    /// [CORE - DDD MARKER] Marks a class as an Aggregate Root in DDD.
    /// Architectural Role: Identifies entities that define consistency boundaries
    /// and are the only entry points for modifications within their aggregate.
    /// </summary>
    public interface IAggregateRoot<TId> : IEquatable<TId> where TId : notnull
    {
        TId Id { get; }

        new bool Equals(TId? other)
        {
            if(other is null)
                return false;

            return Id.Equals(other);
        }
    }
}
