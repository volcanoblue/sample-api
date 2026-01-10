using Moonad;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Query;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    public sealed class GetUserByIdHandler(IUserViewStore store) : IQueryHandler<GetUserByIdQuery, UserView>
    {
        public async Task<Result<UserView, IError>> HandleAsync(GetUserByIdQuery query, CancellationToken ct)
        {
            return await store.GetByIdAsync(query.Id, ct);
        }
    }
}
