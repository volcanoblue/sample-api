using VolcanoBlue.SampleApi.Abstractions;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    public sealed record GetUserByIdQuery(Guid Id) : IQuery;
}