using Moonad;
using System.Threading;
using System.Threading.Tasks;

namespace VolcanoBlue.Core.Command
{
    /// <summary>
    /// [APPLICATION - INPUT PORT] Generic interface for command handlers (use cases that modify state).
    /// Architectural Role: Defines contract for executing commands in Hexagonal Architecture.
    /// Input port that decouples business logic from infrastructure (HTTP, gRPC, CLI, etc).
    /// Uses Railway-Oriented Programming with Result type for explicit error handling.
    /// Generic parameters enable type-safe command handling with specific return and error types.
    /// </summary>
    public interface ICommandHandler<in TCommand, TResult, TError> 
        where TCommand : ICommand
        where TResult : notnull
        where TError : notnull
    {
        Task<Result<TResult, TError>> HandleAsync(TCommand command, CancellationToken ct);
    }
}
