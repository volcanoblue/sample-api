using Moonad;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.Error;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    public static class ChangeEmailEndpoint
    {
        public static WebApplication MapUsersChangeEmail(this WebApplication app)
        {
            app.MapPatch("/users", async (
                HttpContext http,                                             // HTTP context for response generation
                ChangeEmailCommand command,                                   // Automatically deserialized from request body
                ICommandHandler<ChangeEmailCommand, Unit, IError> handler,    // Input port injected by DI
                CancellationToken ct) =>                                      // Cancellation token from HTTP context
            {
                var emailChanged = await handler.HandleAsync(command, ct);
                if (emailChanged)
                {
                    var metrics = app.Services.GetRequiredService<ChangeEmailMetrics>();
                    metrics.EmailChanged.Add(1);
                    
                    return Results.NoContent();
                }

                (int StatusCode, string Message) = emailChanged.ErrorValue switch
                {
                    UserNotFoundError => (StatusCodes.Status404NotFound, "User not found"),
                    EmptyEmailError => (StatusCodes.Status400BadRequest, "Email cannot be empty"),
                    _ => (StatusCodes.Status422UnprocessableEntity, "Unknown error")
                };

                return Results.Problem(ProblemDetailsMapper.FromError(Message, http, StatusCode));
            })
            .WithName("ChangeEmail")                                         // Endpoint name for route linking
            .WithOpenApi()                                                   // Generate OpenAPI documentation
            .Produces<Unit>(StatusCodes.Status200OK)                         // Document 200 response
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);      // Document 422 response

            return app;
        }
    }
}
