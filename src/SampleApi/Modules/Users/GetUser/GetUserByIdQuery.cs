using VolcanoBlue.Core.Query;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    public sealed record GetUserByIdQuery(Guid Id) : IQuery;
}