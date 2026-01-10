using Moonad;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.Error;
using VolcanoBlue.Core.Query;
using VolcanoBlue.EventSourcing.EventStore;
using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.GetUser;
using VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VolcanoBlue.SampleApi.Infrastructure.DependencyInjection
{
    public static class UsersDependencyInjection
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services)
        {
            // OUTPUT PORTS: Register repository implementations
            // Replace by a real one for production
            services.AddDbContext<EventDbContext>(options =>
            {
                options.UseInMemoryDatabase("EventStoreDb");
            });
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
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
