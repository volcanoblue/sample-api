using Moonad;
using System.Threading;
using System.Threading.Tasks;

namespace VolcanoBlue.SampleApi.Abstractions
{
    public interface IQueryHandler<in TQuery, TResult, TError>
        where TQuery : notnull, IQuery
        where TResult : notnull
        where TError : notnull, IError
    {
        Task<Result<Option<TResult>, IError>> HandleAsync(TQuery query, CancellationToken ct);
    }
}
