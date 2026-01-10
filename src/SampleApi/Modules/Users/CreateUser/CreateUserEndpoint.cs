using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    public static class CreateUserEndpoint
    {
        public static WebApplication MapCreateUser(this WebApplication app)
        {
            app.MapPost("/users", async (CreateUserCommand command,           // Automatically deserialized from request body
                ICommandHandler<CreateUserCommand, User, IError> handler,     // Input port injected by DI
                HttpContext context,                                          // HTTP context for response generation
                CancellationToken ct,                                         // Cancellation token from HTTP context
                ILogger<CreateUserCommand> logger) =>                         // Logger injected by DI
            {
                var result = await handler.HandleAsync(command, ct);

                if (result)
                    return Results.Created($"/users/{result.ResultValue.Id}", new { id = result.ResultValue.Id });

                (int StatusCode, string Message) = result.ErrorValue switch
                {
                    EmptyNameError => (StatusCodes.Status400BadRequest, "Name cannot be empty"),
                    EmptyEmailError => (StatusCodes.Status400BadRequest, "Email cannot be empty"),
                    _ => (StatusCodes.Status400BadRequest, "Unknown error occurred")
                };

                return Results.Problem(ProblemDetailsMapper.FromError(Message, context, StatusCode));
                
            })
            .WithName("CreateUser")                                          // Endpoint name for route linking
            .WithOpenApi()                                                   // Generate OpenAPI documentation
            .Produces<object>(StatusCodes.Status201Created)                  // Document 201 response
            .ProducesProblem(StatusCodes.Status400BadRequest)                // Document 400 response
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);      // Document 422 response

            return app;
        }
    }
}