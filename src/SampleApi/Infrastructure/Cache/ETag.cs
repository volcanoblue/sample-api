using System.Text;

namespace VolcanoBlue.SampleApi.Infrastructure.Cache
{
    /// <summary>
    /// [INFRASTRUCTURE] Utility for generating and validating HTTP ETags for cache optimization.
    /// Architectural Role: Implements conditional caching mechanism (HTTP 304 Not Modified)
    /// reducing network traffic by allowing clients to validate if resources have been modified.
    /// </summary>
    public static class ETag
    {
        public static string Create(string source)
        {
            var inputBytes = Encoding.UTF8.GetBytes(source);
            var hexString = Convert.ToHexString(inputBytes);

            return $"\"{hexString}\"";
        }

        public static bool Match(string expectedETag, HttpContext httpContext)
        {
            if(httpContext.Request.Headers.TryGetValue("If-None-Match", out var requestETag))
                return string.Equals(requestETag.ToString(), expectedETag, StringComparison.Ordinal);

            return false;
        }
    }
}
