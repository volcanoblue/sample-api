using Mvc = Microsoft.AspNetCore.Mvc;

namespace VolcanoBlue.SampleApi.Infrastructure.ProblemDetails
{
    /// <summary>
    /// [INFRASTRUCTURE - ERROR MAPPING] Maps domain errors to RFC 7807 Problem Details.
    /// Architectural Role: Translates internal errors to standardized HTTP response format.
    /// Ensures consistency in error communication to API clients.
    /// </summary>
    public static class ProblemDetailsMapper
    {
        public static Mvc.ProblemDetails FromError(string detail,
                                                   HttpContext context,
                                                   int statusCode = StatusCodes.Status400BadRequest)
        {
            var problem = new Mvc.ProblemDetails
            {
                Status = statusCode,
                Title = "A problem occurred",
                Detail = detail,
                Instance = context.Request.Path
            };

            return problem;
        }
    }
}
