using Mvc = Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace VolcanoBlue.SampleApi.Infrastructure.Middleware
{
    public sealed class ExceptionMiddleware(RequestDelegate next, IProblemDetailsService problemDetails)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteProblemDetails(context,
                                          StatusCodes.Status500InternalServerError,
                                          "Internal Server Error",
                                          "An unexpected error occurred",
                                          ex);
            }
        }

        private async Task WriteProblemDetails(HttpContext context, 
                                               int status, 
                                               string title, 
                                               string detail, 
                                               Exception? ex = null)
        {
            context.Response.StatusCode = status;

            var problem = new Mvc.ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            problem.Extensions["traceId"] =
                Activity.Current?.TraceId.ToString()
                ?? context.TraceIdentifier;

            if (ex is not null)
                problem.Extensions["exception"] = ex.GetType().Name;

            await problemDetails.WriteAsync(new ProblemDetailsContext { HttpContext = context, 
                                                                        ProblemDetails = problem });
        }
    }
}
