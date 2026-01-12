using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;
using VolcanoBlue.SampleApi.Modules.Users.GetUser;

namespace VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Endpoints
{
    /// <summary>
    /// [INFRASTRUCTURE - ENDPOINT REGISTRATION] Groups registration of all Users module endpoints.
    /// Architectural Role: Organizes HTTP route mapping in a single point.
    /// Facilitates maintenance and discovery of available endpoints in the module.
    /// </summary>
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
