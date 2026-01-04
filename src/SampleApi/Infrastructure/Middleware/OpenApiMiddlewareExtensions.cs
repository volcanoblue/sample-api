namespace VolcanoBlue.SampleApi.Infrastructure.Middleware
{
    public static class OpenApiMiddlewareExtensions
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
