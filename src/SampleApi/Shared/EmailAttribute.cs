using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VolcanoBlue.SampleApi.Shared
{
    public sealed class EmailAttribute(string? errorMessage = "Invalid e-mail address.") : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null or string { Length: 0 })
                return ValidationResult.Success; // Allow null/empty - use [Required] separately if needed

            if (value is not string email)
                return new ValidationResult(errorMessage);

            if (!EmailAttributeValidator.IsValid(email))
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }
    }

    public static partial class EmailAttributeValidator
    {
        [GeneratedRegex(@"^[a-zA-Z0-9_.][\w\-\._]+[@]{1}[a-z0-9][\w\-]+[\.][a-z]+$", RegexOptions.IgnoreCase)]
        private static partial Regex EmailAddressRegex();

        public static bool IsValid(string input) =>
            EmailAddressRegex().IsMatch(input);
    }
}
