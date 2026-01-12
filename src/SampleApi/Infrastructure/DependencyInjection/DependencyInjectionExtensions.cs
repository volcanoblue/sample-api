namespace VolcanoBlue.SampleApi.Infrastructure.DependencyInjection
{
    /// <summary>
    /// [INFRASTRUCTURE - DEPENDENCY INJECTION] Registers application modules in DI container.
    /// Architectural Role: Composition root that centralizes dependency injection configuration.
    /// Facilitates adding new modules while keeping Program.cs clean.
    /// </summary>
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
