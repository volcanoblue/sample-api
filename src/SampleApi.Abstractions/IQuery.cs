namespace VolcanoBlue.SampleApi.Abstractions
{
    /// <summary>
    /// Marker interface for query operations in CQRS pattern.
    /// 
    /// Architecture Role:
    /// - INPUT PORT: Defines the contract for read operations (Query side of CQRS)
    /// - Queries retrieve data without modifying state
    /// - Segregates read operations from write operations (commands)
    /// 
    /// CQRS Pattern:
    /// Command Query Responsibility Segregation separates read and write operations:
    /// - Commands (ICommand): Modify state, return success/error
    /// - Queries (IQuery): Retrieve data, return data transfer objects
    /// 
    /// Benefits:
    /// ✓ Clear separation between reads and writes
    /// ✓ Optimize read and write operations independently
    /// ✓ Different models for reading vs writing (simplicity vs consistency)
    /// ✓ Easier to scale read operations separately
    /// ✓ Improved performance through specialized read models
    /// 
    /// Usage Pattern:
    /// Implement this interface on query request objects that will be processed
    /// by corresponding IQueryHandler implementations.
    /// 
    /// Comparison with ICommand:
    /// - ICommand: Changes state, returns Unit or entity, uses domain validation
    /// - IQuery: Reads state, returns DTOs, can bypass domain for performance
    /// 
    /// Performance Considerations:
    /// - Queries can use read-optimized data stores (read replicas, caches)
    /// - Can use projections to reduce data transfer
    /// - Can denormalize data for faster reads
    /// - Consider eventual consistency for read models
    /// </summary>
    /// <remarks>
    /// This is a marker interface with no members. Implement it on records or classes
    /// that represent query requests. The actual query handling logic is implemented
    /// in corresponding IQueryHandler implementations.
    /// 
    /// Target Framework: .NET Standard 2.1
    /// C# Version: 8.0
    /// Pattern: CQRS (Command Query Responsibility Segregation)
    /// </remarks>
    public interface IQuery
    {
        // Marker interface - no members required
        // Query implementations should be immutable (use records)
        // Query handlers process these queries and return results
    }
}
