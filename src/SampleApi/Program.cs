using VolcanoBlue.EventSourcing.EventStore.Serialization;
using VolcanoBlue.SampleApi.Infrastructure.DependencyInjection;
using VolcanoBlue.SampleApi.Infrastructure.Middleware;
using VolcanoBlue.SampleApi.Infrastructure.Observability;
using VolcanoBlue.SampleApi.Infrastructure.OpenApi;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();                // Input validation
builder.Services.AddOpenApi();                   // OpenAPI/Swagger documentation
builder.Services.AddObservability();             // Telemetry (traces, metrics, logs)
builder.Services.AddProblemDetailsWithTraceId(); // RFC 7807 Problem Details
builder.Services.AddSwaggerGen();                // Swagger

builder.Services.AddEventSourcing();             // Event Sourcing infrastructure
builder.Services.AddApplication();               // Domain and use case registrations

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();                            // Serve OpenAPI spec in development
}

app.UseHttpsRedirection();                       // Redirect HTTP to HTTPS
app.UseMiddleware<ExceptionMiddleware>();        // Global exception handling

app.MapUsers();                                  // Register User module endpoints

app.Run();

// Expose Program class for integration testing
public partial class Program { }