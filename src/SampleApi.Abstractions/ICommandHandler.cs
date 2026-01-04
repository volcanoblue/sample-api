using Moonad;
using System.Threading;
using System.Threading.Tasks;

namespace VolcanoBlue.SampleApi.Abstractions
{
    /// <summary>
    /// INPUT PORT: Defines the contract for use case execution.
    /// 
    /// Architecture Role:
    /// - PRIMARY PORT (Input Port) - Entry point to the application core
    /// - Invoked by PRIMARY ADAPTERS (REST API, CLI, Message Queues, etc.)
    /// - Contains use case orchestration and business logic
    /// - Depends on OUTPUT PORTS (repositories, external services)
    /// 
    /// Generic Parameters:
    /// - TCommand: The command object containing use case input data
    /// - TResult: The successful outcome type (entity, DTO, Unit)
    /// - TError: The failure type (domain error)
    /// 
    /// Result Type:
    /// Uses Railway-Oriented Programming (Result<TResult, TError>):
    /// - Success track: Contains TResult
    /// - Error track: Contains TError
    /// - Eliminates exceptions for business rule violations
    /// </summary>
    /// <typeparam name="TCommand">Command type implementing ICommand</typeparam>
    /// <typeparam name="TResult">Success result type</typeparam>
    /// <typeparam name="TError">Error result type</typeparam>
    public interface ICommandHandler<in TCommand, TResult, TError> 
        where TCommand : ICommand
        where TResult : notnull
        where TError : notnull
    {
        /// <summary>
        /// Executes the use case asynchronously.
        /// </summary>
        /// <param name="command">Command containing input parameters</param>
        /// <param name="ct">Cancellation token for operation cancellation</param>
        /// <returns>Result containing either success value or error</returns>
        Task<Result<TResult, TError>> HandleAsync(TCommand command, CancellationToken ct);
    }
}
