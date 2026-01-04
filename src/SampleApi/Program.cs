using VolcanoBlue.SampleApi.Infrastructure.DependencyInjection;
using VolcanoBlue.SampleApi.Infrastructure.Middleware;
using VolcanoBlue.SampleApi.Infrastructure.Observability;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using VolcanoBlue.SampleApi.Modules.Users.Infrastructure.Endpoints;

// Create web application builder
var builder = WebApplication.CreateBuilder(args);

// Configure services (Dependency Injection)
builder.Services.AddOpenApi();                   // OpenAPI/Swagger documentation
builder.Services.AddObservability();             // Telemetry (traces, metrics, logs)
builder.Services.AddCustomProblemDetails();      // RFC 7807 Problem Details

builder.Services.AddApplication();               // Domain and use case registrations

// Build application
var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();                            // Serve OpenAPI spec in development
}

app.UseHttpsRedirection();                       // Redirect HTTP to HTTPS
app.UseMiddleware<ExceptionMiddleware>();        // Global exception handling

// Map endpoints (PRIMARY ADAPTERS)
app.MapUsers();                                  // Register user endpoints

// Start application
app.Run();

// Expose Program class for integration testing
public partial class Program { }