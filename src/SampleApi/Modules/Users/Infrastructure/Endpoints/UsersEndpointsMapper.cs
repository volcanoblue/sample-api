using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Endpoints
{
    public static class UsersEndpointsMapper
    {
        public static WebApplication MapUsers(this WebApplication app)
        {
            return app.MapCreateUser()
                      .MapUsersChangeEmail();
        }
    }
}
