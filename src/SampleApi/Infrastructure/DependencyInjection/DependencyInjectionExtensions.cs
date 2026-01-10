namespace VolcanoBlue.SampleApi.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddUsersModule();
            // Add other modules here

            return services;
        }
    }
}
