using Mvc = Microsoft.AspNetCore.Mvc;

namespace VolcanoBlue.SampleApi.Infrastructure.ProblemDetails
{
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
