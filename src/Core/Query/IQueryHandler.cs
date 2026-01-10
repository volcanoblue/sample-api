using Moonad;
using System.Threading;
using System.Threading.Tasks;
using VolcanoBlue.Core.Error;

namespace VolcanoBlue.Core.Query
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : notnull, IQuery
        where TResult : notnull
    {
        Task<Result<TResult, IError>> HandleAsync(TQuery query, CancellationToken ct);
    }
}
