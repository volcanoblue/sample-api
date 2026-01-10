# VolcanoBlue Sample API - An Web API Template for ASP.NET

## Project Overview

This solution demonstrates a clean, maintainable ASP.NET Core 10.0 Web API implementing Hexagonal Architecture (Ports & Adapters) with functional programming principles.

The project showcases modern .NET development practices including:
- Minimal APIs for lightweight HTTP endpoints
- Railway-Oriented Programming for error handling
- OpenTelemetry for observability (traces, metrics, logs)
- RFC 7807 Problem Details for standardized error responses
- Integration testing with WebApplicationFactory
- Event Sourcing for audit trails and temporal queries
- CQRS for separating the domain objects from the view models

### Installation

To install the template you should run the following command: 

`dotnet new install https://github.com/volcanoblue/sample-api`

### Creating a New Project

Run the command below to create a new project based on the template

`dotnet new volcanoblue -n MyCompany.MyApi`

### Template Parameters

```
dotnet new volcanoapi -n Acme.ProductApi 
--api-title "Product Management API" 
--framework net10.0
--include-tests true 
--enable-docker true
```

| Parameter         | Short | Type   | Default        | Description                                    |
|-------------------|-------|--------|----------------|------------------------------------------------|
| `--name`          | `-n`  | string | Directory name | Project name and root namespace                |
| `--framework`     | `-f`  | choice | `net10.0`      | Target framework (`net10`, `net9.0`, `net8.0`) |
| `--include-tests` |       | bool   | `true`         | Include test project                           |
| `--enable-docker` |       | bool   | `true`         | Include Docker files                           |
| `--api-title`     |       | string | "Sample API"   | API title in Swagger                           |
| `--skip-restore`  |       | bool   | `false`        | Skip automatic restore                         |

---

### Swagger
Access Swagger UI at: `https://localhost:<port>/swagger`

## Solution Structure

The solution consists of two main projects:

### 1. VolcanoBlue.SampleApi (Main Application)
- **Target Framework**: .NET 10.0
- **Type**: ASP.NET Core Web API
- **Entry Point**: Program.cs
- **Docker Support**: Linux containers

### 2. VolcanoBlue.Core (Shared Contracts)
- **Target Framework**: .NET Standard 2.1
- **Contains**: Interfaces, abstractions, shared types
- **Purpose**: Define contracts between layers

### 3. VolcanoBlue.EventSourcing (Event Sourcing Infrastructure)
- **Target Framework**: .NET 10
- **Contains**: Event Store, Event Sourced Entities, Event Serialization
- **Purpose**: Provide event sourcing capabilities for domain aggregates

---

## Architectural Patterns & Advantages

### Hexagonal Architecture (Ports & Adapters)

The application is organized in distinct layers with clear boundaries:
- **Core Domain** (center): Pure business logic with no external dependencies
- **Application Layer**: Use case orchestration
- **Adapters Layer**: External integrations (HTTP, database, etc.)

**Advantages:**

✓ **Technology Independence**
  - Business logic doesn't depend on frameworks, databases, or UI
  - Easy to migrate from ASP.NET to another framework without touching core logic
  - Domain code can be reused across different platforms

✓ **Testability**
  - Core business logic can be tested without HTTP, database, or infrastructure
  - Easy to create test doubles for external dependencies
  - Unit tests run fast without requiring external resources

✓ **Flexibility in Adapters**
  - Multiple adapters can invoke the same use case: REST API endpoint, gRPC service, CLI command, or message queue consumer
  - Easy to add new integration points without modifying business logic

✓ **Clear Separation of Concerns**
  - Each layer has a single, well-defined responsibility
  - Changes in one layer don't cascade to others
  - Team members can work on different layers independently

✓ **Maintainability**
  - Easy to locate where changes should be made
  - Reduced coupling between components
  - Better code organization and discoverability

---

### Event Sourcing

Event Sourcing is a pattern where state changes are stored as a sequence of events rather than storing just the current state. This solution includes a complete Event Sourcing infrastructure that can be used to build event-sourced aggregates.

**Advantages:**

✓ **Complete Audit Trail**
  - Every state change is recorded as an immutable event
  - Full history of what happened, when, and why
  - Compliance and regulatory requirements easily met

✓ **Temporal Queries**
  - Reconstruct entity state at any point in time
  - Analyze historical trends and patterns
  - Debug production issues by replaying events

✓ **Scalability**
  - Append-only storage (no updates or deletes)
  - Optimized for write performance
  - Read models can be optimized independently

✓ **Business Insights**
  - Events capture business intent, not just final state
  - Rich domain events enable analytics
  - Event stream as source of truth

---

### Railway-Oriented Programming (Result Pattern)

Functions return Result types instead of throwing exceptions for expected error scenarios.

**Advantages:**

✓ **Explicit Error Handling**
  - Errors are part of the return type, making them visible in signatures
  - Compiler forces you to handle both success and error cases
  - No hidden control flow through exceptions

✓ **Composability**
  - Results can be chained using functional operators
  - Early returns on first error (fail-fast)
  - Reduces nested try-catch blocks

✓ **Type Safety**
  - Specific error types for different failure scenarios
  - Pattern matching provides exhaustive error handling
  - Compile-time guarantees about error scenarios

✓ **Performance**
  - No expensive exception throwing/catching
  - Exceptions break CPU pipeline prediction
  - Result types are more predictable and faster

✓ **Better API Design**
  - Callers know what errors to expect
  - Self-documenting code through error types
  - Reduces defensive programming

---

### Command Handler Pattern (Use Cases)

Each use case implemented as separate handler implementing a common interface.

**Advantages:**

✓ **Single Responsibility Principle**
  - Each handler does one thing
  - Easy to understand and maintain
  - Changes isolated to specific handlers

✓ **Explicit Use Cases**
  - Business capabilities clearly visible
  - Each handler represents a user story
  - Easy to map requirements to code

✓ **Testability**
  - Test each use case independently
  - Mock dependencies per handler
  - Clear test scenarios

✓ **Extensibility**
  - Add new use cases without modifying existing code
  - No God classes with multiple responsibilities
  - Open/Closed Principle adherence

✓ **Decorator Pattern Support**
  - Easy to add cross-cutting concerns: logging, validation, transactions, caching
  - Wrap handlers without modifying their code
  - Composable behavior

---

### Minimal APIs

Endpoints defined using Minimal APIs instead of controllers.

**Advantages:**

✓ **Less Ceremony**
  - No need for controller classes
  - Direct route-to-handler mapping
  - Reduced boilerplate code

✓ **Performance**
  - Faster startup time
  - Lower memory footprint
  - Source generated route handlers in .NET 9

✓ **Functional Approach**
  - Endpoints as functions
  - Easy to understand request/response flow
  - Better for simple APIs

✓ **Simplified Testing**
  - Test endpoints with WebApplicationFactory
  - Less framework overhead
  - Faster integration tests

✓ **Modern .NET Features**
  - Built-in dependency injection
  - Automatic parameter binding
  - Native OpenAPI support

---

### OpenTelemetry Observability

Integrated distributed tracing, metrics, and logging.

**Advantages:**

✓ **Vendor Neutrality**
  - Not locked to specific monitoring tool
  - Export to any OTLP-compatible backend
  - Future-proof observability strategy

✓ **Comprehensive Visibility**
  - Traces: Request flow through system
  - Metrics: Performance counters and statistics
  - Logs: Structured event data
  - Correlated through trace IDs

✓ **Production Debugging**
  - Identify bottlenecks in request pipeline
  - Track errors across service boundaries
  - Performance monitoring without code changes

✓ **Standardization**
  - Industry standard (CNCF project)
  - Consistent approach across services
  - Better tooling support

---

### RFC 7807 Problem Details

Standardized error response format across all API endpoints.

**Advantages:**

✓ **Consistency**
  - All errors use same structure
  - Predictable for API clients
  - Easy to parse and handle

✓ **Rich Error Information**
  - Type: Error category URL
  - Title: Human-readable summary
  - Detail: Specific error description
  - Instance: Request path that caused error
  - Extensions: Custom fields (traceId, exception type)

✓ **Industry Standard**
  - RFC standard format
  - Client libraries understand format
  - Better API documentation

✓ **Debugging Support**
  - Trace ID links errors to telemetry
  - Exception type aids troubleshooting
  - Instance shows exact endpoint

---

### Global Exception Middleware

Centralized exception handling at application level.

**Advantages:**

✓ **Consistent Error Handling**
  - All unhandled exceptions processed uniformly
  - No try-catch scattered through code
  - Single place to modify error responses

✓ **Security**
  - Prevents leaking stack traces to clients
  - Controlled error information exposure
  - Consistent responses prevent information disclosure

✓ **Observability**
  - All exceptions logged automatically
  - Trace IDs added to all error responses
  - Easy to monitor error rates

✓ **Separation of Concerns**
  - Business logic doesn't handle HTTP errors
  - Infrastructure concern handled in infrastructure layer
  - Cleaner handler code

---

## Sample Domain Model

### User Entity (Aggregate Root)

**Properties:**
- Id: Unique identifier
- Name: User's name
- Email: User's email address

**Business Rules:**
- Name cannot be null or empty
- Email cannot be null or empty
- User creation goes through factory method
- Email changes go through domain method

**Encapsulation Benefits:**
- Private setters prevent invalid state changes
- Public methods enforce business rules
- Impossible to bypass validation
- Self-contained business logic

### Create User

- **Handler**: CreateUserHandler
- **Command**: Name and Email
- **Endpoint**: POST /users
- **Success Response**: 201 Created with User ID
- **Error Responses**: 
  - 400 Bad Request (invalid data)
  - 422 Unprocessable Entity (cancellation requested)

**Workflow:**
1. Validate command data through domain
2. Create User entity via factory method
3. Save user via repository
4. Return created user

**Benefits:**
- Handler is reusable from any adapter (HTTP, CLI, queue)
- Easy to test without HTTP infrastructure
- Clear separation of validation (domain) and orchestration (handler)

---

### Change Email

- **Handler**: ChangeEmailHandler
- **Command**: User ID and New Email
- **Endpoint**: PATCH /users/{id}/email
- **Success Response**: 204 No Content
- **Error Responses**:
  - 400 Bad Request (invalid email)
  - 404 Not Found (user doesn't exist)

**Benefits:**
- Email validation in domain layer
- Handler orchestrates: fetch user, change email, save user
- Repository abstraction allows different persistence strategies

---

## Infrastructure

### Dependency Injection

Registers all application services:
- Command handlers (scoped lifetime)
- Repositories (singleton for InMemory, scoped for Database)
- Metrics collectors

**Configuration Strategy:**
- Development: InMemoryUserRepository
- Production: DatabaseUserRepository (needs to be implemented)

**Benefits:**
- Change from in-memory to database requires one line change
- All handlers automatically use new repository
- No code changes in domain or application layer

---

## Dependencies

### Main Project (VolcanoBlue.SampleApi)

- **Microsoft.AspNetCore.OpenApi** 9.0.0 - OpenAPI/Swagger documentation
- **Swashbuckle.AspNetCore** 9.0.6 - Swagger UI
- **Moonad** 5.5.1 - Functional programming utilities for Result types
- **OpenTelemetry.*** 1.14.0 - Observability stack (tracing, metrics, logging)
- **Microsoft.VisualStudio.Azure.Containers.Tools.Targets** 1.22.1 - Docker tooling

---

## Extending the Application

### Adding a New Use Case
1. Create domain entity/value object in Domain folder
2. Define command record
3. Implement command handler interface
4. Register handler in dependency injection configuration
5. Create endpoint mapper in infrastructure
6. Map endpoint in application startup

**Benefits**: New features don't modify existing code (Open/Closed Principle)

### Adding Database Persistence
1. Create database repository implementing repository interface
2. Add Entity Framework Core packages
3. Configure database context
4. Update dependency injection to register database context
5. Change repository registration to database implementation

**Benefits**: Business logic remains unchanged, only adapter switches

---

## Troubleshooting

### Build Errors
- Ensure .NET 10.0 SDK is installed
- Restore NuGet packages

### Runtime Errors
- Check logs in debug output
- Verify configuration files
- Check trace IDs in error responses for correlation

### OpenTelemetry Issues
- Ensure OTLP endpoint is configured (if using external collector)
- Verify metrics collection is working

---

## Version Information

- **.NET Version**: 10.0
- **.NET Standard**: 2.1 (Abstractions project)
- **C# Language Version**: Latest (implied by .NET 9)
- **Nullable Reference Types**: Enabled
- **Implicit Usings**: Enabled

---

**Last Updated**: January 2026
