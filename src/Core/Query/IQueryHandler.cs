using Moonad;
using System.Threading;
using System.Threading.Tasks;
using VolcanoBlue.Core.Error;

namespace VolcanoBlue.Core.Query
{
    /// <summary>
    /// [APPLICATION - INPUT PORT] Generic interface for query handlers (use cases that read data).
    /// Architectural Role: Defines contract for executing queries in Hexagonal Architecture.
    /// Separates read operations from write operations (CQRS pattern).
    /// Input port that decouples query logic from infrastructure adapters.
    /// Uses Railway-Oriented Programming with Result type for explicit error handling.
    /// All query handlers return IError as standardized error type.
    /// </summary>
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : notnull, IQuery
        where TResult : notnull
    {
        Task<Result<TResult, IError>> HandleAsync(TQuery query, CancellationToken ct);
    }
}
