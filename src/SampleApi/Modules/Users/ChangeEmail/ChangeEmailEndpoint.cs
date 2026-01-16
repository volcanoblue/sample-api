using Moonad;
using VolcanoBlue.Core.Command;
using VolcanoBlue.Core.Error;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Domain.Errors;
using VolcanoBlue.SampleApi.Modules.Users.Domain.ValueObjects;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// [INFRASTRUCTURE - PRIMARY ADAPTER] HTTP endpoint for email change.
    /// Architectural Role: Primary adapter that exposes use case via HTTP PATCH.
    /// Records observability metrics after successful execution.
    /// </summary>
    public static class ChangeEmailEndpoint
    {
        public static WebApplication MapUsersChangeEmail(this WebApplication app)
        {
            app.MapPost("/users/change-email", async (
                HttpContext http,                                             // HTTP context for response generation
                ChangeEmailCommand command,                                   // Automatically deserialized from request body
                ICommandHandler<ChangeEmailCommand, Unit, IError> handler,    // Input port injected by DI
                CancellationToken ct,
                ChangeEmailMetrics metrics) =>                                // Cancellation token from HTTP context
            {
                var emailChanged = await handler.HandleAsync(command, ct);
                if (emailChanged)
                {
                    metrics.EmailChanged.Add(1);
                    
                    return Results.NoContent();
                }

                (int StatusCode, string Detail) = emailChanged.ErrorValue switch
                {
                    UserNotFoundError => (StatusCodes.Status404NotFound, "User not found"),
                    InvalidEmailError => (StatusCodes.Status400BadRequest, "Email cannot be empty"),
                    _ => (StatusCodes.Status422UnprocessableEntity, "Unknown error")
                };

                return Results.Problem(ProblemDetailsFactory.Create(http, StatusCode, "Business Error", Detail));
            })
            .WithName("ChangeEmail")                                         // Endpoint name for route linking
            .WithOpenApi()                                                   // Generate OpenAPI documentation
            .Produces<Unit>(StatusCodes.Status200OK)                         // Document 200 response
            .Produces(StatusCodes.Status204NoContent)                        // Document 200 response               
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);      // Document 422 response

            return app;
        }
    }
}
