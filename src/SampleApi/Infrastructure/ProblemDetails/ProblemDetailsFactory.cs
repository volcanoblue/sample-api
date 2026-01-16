using Microsoft.Extensions.ObjectPool;
using Mvc = Microsoft.AspNetCore.Mvc;

namespace VolcanoBlue.SampleApi.Infrastructure.ProblemDetails
{
    /// <summary>
    /// [INFRASTRUCTURE - ERROR MAPPING] Maps domain errors to RFC 7807 Problem Details.
    /// Architectural Role: Translates internal errors to standardized HTTP response format.
    /// Ensures consistency in error communication to API clients.
    /// Performance: Uses ObjectPool to reduce allocations for frequently created ProblemDetails objects.
    /// </summary>
    public static class ProblemDetailsFactory
    {
        private static readonly ObjectPool<Mvc.ProblemDetails> _pool = ObjectPool.Create(new ProblemDetailsPooledObjectPolicy());

        public static Mvc.ProblemDetails Create(HttpContext context,
                                                int statusCode = StatusCodes.Status400BadRequest,
                                                string title = "",
                                                string detail = "")
        {
            var problem = new Mvc.ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            return problem;
        }
    }
}
