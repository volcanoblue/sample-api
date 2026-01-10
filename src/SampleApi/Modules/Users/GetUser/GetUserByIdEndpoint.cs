using VolcanoBlue.Core.Query;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.GetUser
{
    public static class GetUserByIdEndpoint
    {
        public static WebApplication MapGetUserById(this WebApplication app)
        {
            app.MapGet("/users/{id:guid}", async (Guid id,                               // Route parameter: user ID extracted from URL
                IQueryHandler<GetUserByIdQuery, UserView> handler,                       // Input port injected by DI
                CancellationToken ct) =>                                                 // Cancellation token from HTTP context
            {
                var result = await handler.HandleAsync(new GetUserByIdQuery(id), ct);
                
                if (result.IsError)
                    return result.ErrorValue is UserViewNotFoundError
                        ? Results.NotFound()
                        : Results.Problem(statusCode: StatusCodes.Status500InternalServerError);

                return Results.Ok(result.ResultValue);
            })
            .WithName("GetUserById")                                                      // Endpoint name for route linking and OpenAPI
            .Produces<UserView>(200)                                                      // Document 200 response with UserView schema
            .Produces(404)                                                                // Document 404 response (empty body)
            .ProducesProblem(500);                                                        // Document 500 response with ProblemDetails

            return app;                                                                   // Return app for method chaining
        }
    }
}
