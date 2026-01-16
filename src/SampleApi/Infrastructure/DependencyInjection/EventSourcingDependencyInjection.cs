using Microsoft.EntityFrameworkCore;
using VolcanoBlue.EventSourcing.EventStore;
using VolcanoBlue.EventSourcing.EventStore.Serialization;

namespace VolcanoBlue.SampleApi.Infrastructure.DependencyInjection
{
    public static class EventSourcingDependencyInjection
    {
        public static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            // OUTPUT PORTS: Register repository implementations
            // Replace by a real one for production
            services.AddDbContext<EventDbContext>(options =>
            {
                options.UseInMemoryDatabase("EventStoreDb");
            });

            // Scan assemblies once at startup to find IEvent implementations
            EventTypeRegistry.Initialize(typeof(Program).Assembly);

            return services;
        }
    }
}
