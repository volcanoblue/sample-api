namespace VolcanoBlue.SampleApi.Infrastructure.OpenApi
{
    /// <summary>
    /// [INFRASTRUCTURE - API DOCUMENTATION] Configures Swagger/OpenAPI for API documentation.
    /// Architectural Role: Exposes interactive interface for exploring and testing endpoints.
    /// Generates OpenAPI specification automatically from configured endpoints.
    /// </summary>
    public static class OpenApiExtensions
    {
        public static WebApplication UseOpenApi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API v1");
            });

            return app;
        }
    }
}
