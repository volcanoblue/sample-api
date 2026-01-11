using System.Security.Cryptography;
using System.Text;

namespace VolcanoBlue.SampleApi.Infrastructure.Cache
{
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
