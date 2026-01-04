using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence;

namespace VolcanoBlue.SampleApi.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Dependency injection configuration for the application.
    /// 
    /// Architecture Role:
    /// - Wires INPUT PORTS to implementations
    /// - Wires OUTPUT PORTS to adapters
    /// - Composition root: Where dependencies are configured
    /// 
    /// Registration Patterns:
    /// - Singleton: Single instance for app lifetime (repositories, metrics)
    /// - Scoped: Instance per HTTP request (handlers)
    /// - Transient: New instance every time (rarely used)
    /// 
    /// Hexagonal Architecture Benefits:
    /// - Easy to swap implementations (change one line)
    /// - Test configurations (use test doubles)
    /// - Feature flags (conditional registration)
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // OUTPUT PORTS: Register repository implementations
            // Change to DatabaseUserRepository for production
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();

            // Metrics (observability concern)
            services.AddSingleton<ChangeEmailMetrics>();

            // INPUT PORTS: Register command handlers (use cases)
            services.AddScoped<ICommandHandler<CreateUserCommand, User, IError>, CreateUserHandler>();
            services.AddScoped<ICommandHandler<ChangeEmailCommand, Unit, IError>, ChangeEmailHandler>();

            return services;
        }
    }
}
