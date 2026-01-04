using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.CreateUser
{
    /// <summary>
    /// PRIMARY ADAPTER: REST API endpoint for user creation using Minimal APIs.
    /// 
    /// Architecture Role:
    /// - PRIMARY ADAPTER (Driving side) - Receives HTTP requests from external world
    /// - Invokes INPUT PORT: ICommandHandler (use case)
    /// - Translates HTTP concerns into domain operations
    /// 
    /// Responsibilities:
    /// 1. HTTP routing and verb mapping (POST /users)
    /// 2. Request deserialization (JSON to CreateUserCommand)
    /// 3. Invoking the use case through the input port
    /// 4. Translating domain results into HTTP responses
    /// 5. OpenAPI documentation metadata
    /// 
    /// This adapter can be replaced with other primary adapters without changing business logic:
    /// - gRPC service
    /// - GraphQL resolver
    /// - Message queue consumer
    /// - CLI command handler
    /// 
    /// Hexagonal Architecture Benefit:
    /// The use case (CreateUserHandler) remains unchanged regardless of how it's invoked.
    /// </summary>
    public static class CreateUserEndpoint
    {
        /// <summary>
        /// Extension method that maps the user creation endpoint to the application pipeline.
        /// Uses the fluent API pattern for clean endpoint configuration.
        /// </summary>
        /// <param name="app">The WebApplication instance to configure</param>
        /// <returns>The WebApplication for method chaining</returns>
        public static WebApplication MapCreateUser(this WebApplication app)
        {
            app.MapPost("/users", async (CreateUserCommand command,           // Automatically deserialized from request body
                ICommandHandler<CreateUserCommand, User, IError> handler,     // Input port injected by DI
                HttpContext context,                                          // HTTP context for response generation
                CancellationToken ct) =>                                      // Cancellation token from HTTP context
            {
                // Invoke the use case through the input port
                var result = await handler.HandleAsync(command, ct);

                // Translate domain result to HTTP response
                if (result)
                    return Results.Created($"/users/{result.ResultValue.Id}", new { id = result.ResultValue.Id });

                // Error path: Map domain errors to HTTP status codes
                // Pattern matching: Specific error types to appropriate HTTP responses
                (int StatusCode, string Message) = result.ErrorValue switch
                {
                    CancellationRequestedError => (StatusCodes.Status422UnprocessableEntity, "Cancellation requested"),
                    EmptyNameError => (StatusCodes.Status400BadRequest, "Name cannot be empty"),
                    EmptyEmailError => (StatusCodes.Status400BadRequest, "Email cannot be empty"),
                    _ => (StatusCodes.Status400BadRequest, "Unknown error occurred")
                };

                // Return RFC 7807 Problem Details with appropriate status code
                return Results.Problem(ProblemDetailsMapper.FromError(Message, context, StatusCode));
            })
            .WithName("CreateUser")                                          // Endpoint name for route linking
            .WithOpenApi()                                                   // Generate OpenAPI documentation
            .Produces<object>(StatusCodes.Status201Created)                  // Document 201 response
            .ProducesProblem(StatusCodes.Status400BadRequest);               // Document 400 response

            return app;
        }
    }
}