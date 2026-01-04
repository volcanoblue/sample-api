using Microsoft.Extensions.ObjectPool;
using Mvc = Microsoft.AspNetCore.Mvc;

namespace VolcanoBlue.SampleApi.Infrastructure.ProblemDetails
{
    /// <summary>
    /// Maps domain errors to RFC 7807 Problem Details for HTTP responses.
    /// 
    /// Architecture Role:
    /// - Infrastructure concern (HTTP translation)
    /// - Translates domain errors (IError) to HTTP format
    /// - Used by PRIMARY ADAPTERS (endpoints)
    /// 
    /// RFC 7807 Problem Details:
    /// Standard JSON format for HTTP API errors:
    /// {
    ///   "type": "about:blank",
    ///   "title": "Business rule violation",
    ///   "detail": "Email cannot be empty",
    ///   "status": 400,
    ///   "instance": "/users"
    /// }
    /// 
    /// Benefits:
    /// 1. Standardized error format
    /// 2. Machine-readable errors
    /// 3. Human-readable messages
    /// 4. Consistent across all endpoints
    /// </summary>
    public static class ProblemDetailsMapper
    {
        private static readonly ObjectPool<Mvc.ProblemDetails> _pool = ObjectPool.Create<Mvc.ProblemDetails>();

        /// <summary>
        /// Converts a domain error to HTTP Problem Details.
        /// </summary>
        /// <param name="context">HTTP context for request details</param>
        /// <param name="statusCode">HTTP status code (default 400)</param>
        /// <returns>ProblemDetails object for HTTP response</returns>
        public static Mvc.ProblemDetails FromError(string detail,
                                                   HttpContext context,
                                                   int statusCode = StatusCodes.Status400BadRequest)
        {
            var problem = _pool.Get();
            try
            {
                problem.Status = statusCode;
                problem.Title = "Business rule violation";
                problem.Detail = detail;
                problem.Instance = context.Request.Path;
                
                return problem;
            }
            catch
            {
                _pool.Return(problem);
                throw;
            }
        }
    }
}
