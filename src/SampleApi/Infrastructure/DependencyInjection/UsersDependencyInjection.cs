using Moonad;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Query;
using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.GetUser;
using VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence;

namespace VolcanoBlue.SampleApi.Infrastructure.DependencyInjection
{
    /// <summary>
    /// [INFRASTRUCTURE - DEPENDENCY INJECTION] Registers Users module services in container.
    /// Architectural Role: Configures input ports (handlers), output ports (repositories) and cross-cutting concerns (metrics).
    /// Centralizes Users module dependency configuration allowing easy implementation substitution.
    /// </summary>
    public static class UsersDependencyInjection
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, InMemoryUserRepository>();
            services.AddSingleton<IUserViewStore, InMemoryUserViewStorage>();
            
            // Metrics (observability concern)
            services.AddSingleton<ChangeEmailMetrics>();
            
            // INPUT PORTS: Register command handlers (use cases)
            services.AddScoped<ICommandHandler<CreateUserCommand, User, IError>, CreateUserHandler>();
            services.AddScoped<ICommandHandler<ChangeEmailCommand, Unit, IError>, ChangeEmailHandler>();
            services.AddScoped<IQueryHandler<GetUserByIdQuery, UserView>, GetUserByIdHandler>();

            return services;
        }
    }
}
