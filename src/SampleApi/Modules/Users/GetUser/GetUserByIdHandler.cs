using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.Domain;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    public sealed class GetUserByIdHandler(IUserViewStore store) : IQueryHandler<GetUserByIdQuery, UserView, IError>
    {
        public async Task<Result<Option<UserView>, IError>> HandleAsync(GetUserByIdQuery query, CancellationToken ct)
        {
            return await store.GetByIdAsync(query.Id, ct);
        }
    }
}
