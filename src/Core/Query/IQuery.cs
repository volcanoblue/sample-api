namespace VolcanoBlue.Core.Query
{
    /// <summary>
    /// [APPLICATION - MARKER INTERFACE] Base marker interface for all queries in the system.
    /// Architectural Role: Identifies query objects in CQRS pattern. Queries represent read operations
    /// that don't modify state. Marker interface enables generic constraint on IQueryHandler.
    /// </summary>
    public interface IQuery
    {
        
    }
}
