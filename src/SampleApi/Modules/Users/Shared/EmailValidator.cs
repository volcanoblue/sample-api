using System.Text.RegularExpressions;

namespace VolcanoBlue.SampleApi.Shared
{
    /// <summary>
    /// [SHARED - VALIDATION UTILITY] Provides centralized email format validation using compiled regex patterns.
    /// Architectural Role: Shared kernel utility for validating email address format across all application layers.
    /// </summary>
    public static partial class EmailValidator
    {
        [GeneratedRegex(@"^[a-zA-Z0-9_.][\w\-\._]+[@]{1}[a-z0-9][\w\-]+[\.][a-z]+$", RegexOptions.IgnoreCase)]
        private static partial Regex EmailAddressRegex();

        public static bool IsInvalid(string input)
        {
            var invalid = !EmailAddressRegex().IsMatch(input);
            return invalid;
        }
    }
}
