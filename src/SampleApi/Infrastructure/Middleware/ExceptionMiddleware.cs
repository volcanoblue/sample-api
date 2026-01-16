using Microsoft.AspNetCore.Mvc.Infrastructure;
using VolcanoBlue.SampleApi.Infrastructure.ProblemDetails;
using Mvc = Microsoft.AspNetCore.Mvc;

namespace VolcanoBlue.SampleApi.Infrastructure.Middleware
{
    /// <summary>
    /// [INFRASTRUCTURE - MIDDLEWARE] Global middleware for capturing unhandled exceptions.
    /// Architectural Role: Ensures unexpected exceptions are converted to RFC 7807 Problem Details responses.
    /// Prevents sensitive information leakage and maintains consistency in error responses.
    /// </summary>
    public sealed class ExceptionMiddleware(RequestDelegate next, 
                                            IProblemDetailsService problemDetailsService)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception exc)
            {
                await WriteProblemDetailsAsync(context, exc);
            }
        }

        private async Task WriteProblemDetailsAsync(HttpContext context, Exception exc)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var problem = ProblemDetailsMapper.Map(context,
                                                   context.Response.StatusCode,
                                                   "Internal Server Error",
                                                   "An unexpected error occurred");

            problem.Extensions["exception"] = exc.GetType().Name;

            await problemDetailsService.WriteAsync(new ProblemDetailsContext { HttpContext = context, ProblemDetails = problem });
        }
    }
}
