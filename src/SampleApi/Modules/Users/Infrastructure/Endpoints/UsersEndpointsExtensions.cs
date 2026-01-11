using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;
using VolcanoBlue.SampleApi.Modules.Users.GetUser;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Endpoints
{
    public static class UsersEndpointsExtensions
    {
        public static WebApplication MapUsers(this WebApplication app)
        {
            return app.MapCreateUser()
                      .MapUsersChangeEmail()
                      .MapGetUserById();
        }
    }
}
