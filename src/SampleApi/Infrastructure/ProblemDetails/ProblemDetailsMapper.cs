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
    public static class ProblemDetailsMapper
    {
        private static readonly ObjectPool<Mvc.ProblemDetails> _pool = ObjectPool.Create(new ProblemDetailsPooledObjectPolicy());

        public static Mvc.ProblemDetails Map(HttpContext context,
                                             int statusCode = StatusCodes.Status400BadRequest,
                                             string title = "",
                                             string detail = "")
        {
            var problem = _pool.Get();

            try 
            {
                problem.Status = statusCode;
                problem.Title = title;
                problem.Detail = detail;
                problem.Instance = context.Request.Path;
            }
            finally
            {
                _pool.Return(problem);
            }

            return problem;
        }
    }
}
