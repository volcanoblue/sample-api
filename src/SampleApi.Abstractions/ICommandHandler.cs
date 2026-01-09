using Moonad;
using System.Threading;
using System.Threading.Tasks;

namespace VolcanoBlue.SampleApi.Abstractions
{
    public interface ICommandHandler<in TCommand, TResult, TError> 
        where TCommand : ICommand
        where TResult : notnull
        where TError : notnull
    {
        Task<Result<TResult, TError>> HandleAsync(TCommand command, CancellationToken ct);
    }
}
