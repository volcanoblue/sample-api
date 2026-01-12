using VolcanoBlue.Core.Query;
using VolcanoBlue.SampleApi.Infrastructure.Cache;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    /// <summary>
    /// [INFRASTRUCTURE - PRIMARY ADAPTER] HTTP endpoint for user query.
    /// Architectural Role: Primary adapter for queries. Implements HTTP caching with ETags
    /// to reduce traffic (returns 304 Not Modified when resource hasn't changed).
    /// </summary>
    public static class GetUserByIdEndpoint
    {
        public static WebApplication MapGetUserById(this WebApplication app)
        {
            app.MapGet("/users/{id:guid}", async (Guid id,                               // Route parameter: user ID extracted from URL
                HttpContext httpContext,                                                 // HTTP context for accessing request info
                IQueryHandler<GetUserByIdQuery, UserView> handler,                       // Input port injected by DI
                CancellationToken ct) =>                                                 // Cancellation token from HTTP context
            {
                var result = await handler.HandleAsync(new GetUserByIdQuery(id), ct);
                if (result)
                {
                    UserView view = result.ResultValue;
                    var eTag = ETag.Create($"{view.Id}-{view.Email}");                   // Create ETag based on user's email since it can change

                    if (ETag.Match(eTag, httpContext))
                        return Results.StatusCode(StatusCodes.Status304NotModified);

                    httpContext.Response.Headers.ETag = eTag;                            // Set ETag header for caching
                    return Results.Ok(result.ResultValue);
                }

                return result.ErrorValue is UserViewNotFoundError
                        ? Results.NotFound()
                        : Results.Problem(statusCode: StatusCodes.Status500InternalServerError);
            })
            .WithName("GetUserById")                                                      // Endpoint name for route linking and OpenAPI
            .Produces<UserView>(200)                                                      // Document 200 response with UserView schema
            .Produces(304)                                                                // Document 304 response for caching  
            .Produces(404)                                                                // Document 404 response (empty body)
            .ProducesProblem(500);                                                        // Document 500 response with ProblemDetails

            return app;                                                                   // Return app for method chaining
        }
    }
}
