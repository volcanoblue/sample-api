using Moonad;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Query;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    /// <summary>
    /// [APPLICATION - USE CASE] Handler for querying user by ID.
    /// Architectural Role: Orchestrates fetch from optimized read model.
    /// Implements read side of CQRS pattern, separated from command handlers.
    /// </summary>
    public sealed class GetUserByIdHandler(IUserViewStore store) : IQueryHandler<GetUserByIdQuery, UserView>
    {
        public async Task<Result<UserView, IError>> HandleAsync(GetUserByIdQuery query, CancellationToken ct)
        {
            return await store.GetByIdAsync(query.Id, ct);
        }
    }
}
