using Moonad;
using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.ChangeEmail
{
    /// <summary>
    /// PRIMARY ADAPTER: REST API endpoint for changing user email using Minimal APIs.
    /// 
    /// Architecture Role:
    /// - PRIMARY ADAPTER (Driving side) - Receives HTTP requests from external world
    /// - Invokes INPUT PORT: ICommandHandler (use case)
    /// - Translates HTTP concerns into domain operations
    /// 
    /// Responsibilities:
    /// 1. HTTP routing and verb mapping (POST /users/change-email)
    /// 2. Request deserialization (JSON to ChangeEmailCommand)
    /// 3. Invoking the use case through the input port
    /// 4. Recording metrics (observability concern)
    /// 5. Translating domain results into HTTP responses
    /// 6. Error-specific HTTP status code mapping
    /// 
    /// This adapter can be replaced with other primary adapters without changing business logic:
    /// - gRPC service
    /// - GraphQL mutation
    /// - Message queue consumer
    /// - CLI command handler
    /// 
    /// Hexagonal Architecture Benefit:
    /// The use case (ChangeEmailHandler) remains unchanged regardless of how it's invoked.
    /// </summary>
    public static class ChangeEmailEndpoint
    {
        /// <summary>
        /// Extension method that maps the change email endpoint to the application pipeline.
        /// Uses the fluent API pattern for clean endpoint configuration.
        /// </summary>
        /// <param name="app">The WebApplication instance to configure</param>
        /// <returns>The WebApplication for method chaining</returns>
        public static WebApplication MapUsersChangeEmail(this WebApplication app)
        {
            app.MapPost("/users/change-email", async (
                HttpContext http,                                             // HTTP context for response generation
                ChangeEmailCommand command,                                   // Automatically deserialized from request body
                ICommandHandler<ChangeEmailCommand, Unit, IError> handler,    // Input port injected by DI
                CancellationToken ct) =>                                      // Cancellation token from HTTP context
            {
                // Invoke the use case through the input port
                var emailChanged = await handler.HandleAsync(command, ct);
                
                // Success path: Record metrics and return 200 OK
                if (emailChanged)
                {
                    // Infrastructure concern: Record metric for observability
                    var metrics = app.Services.GetRequiredService<ChangeEmailMetrics>();
                    metrics.EmailChanged.Add(1);
                    
                    return Results.Ok();
                }

                // Error path: Map domain errors to HTTP status codes
                // Pattern matching: Specific error types to appropriate HTTP responses
                (int StatusCode, string Message) = emailChanged.ErrorValue switch
                {
                    CancellationRequestedError => (StatusCodes.Status422UnprocessableEntity, "Cancellation requested"),
                    UserNotFoundError => (StatusCodes.Status404NotFound, "User not found"),
                    EmptyEmailError => (StatusCodes.Status400BadRequest, "Email cannot be empty"),
                    _ => (StatusCodes.Status400BadRequest, "Unknown error")
                };

                // Return RFC 7807 Problem Details with appropriate status code
                return Results.Problem(ProblemDetailsMapper.FromError(Message, http, StatusCode));
            })
            .WithName("ChangeEmail")                                         // Endpoint name for route linking
            .WithOpenApi()                                                   // Generate OpenAPI documentation
            .Produces<object>(StatusCodes.Status200OK)                       // Document 200 response
            .ProducesProblem(StatusCodes.Status400BadRequest);               // Document 400 response

            return app;
        }
    }
}
