namespace VolcanoBlue.SampleApi.Infrastructure.ProblemDetails
{
    public static class ProblemDetailsExtension
    {
        public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context => 
                    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
            });

            return services;
        }
    }
}
