using System.ComponentModel.DataAnnotations;
using VolcanoBlue.SampleApi.Shared;

namespace VolcanoBlue.SampleApi.Modules.Users.Shared
{
    /// <summary>
    /// [INFRASTRUCTURE - VALIDATION ATTRIBUTE] Custom validation attribute for ASP.NET data annotations.
    /// Architectural Role: API/Infrastructure layer validation for request DTOs, delegating to EmailValidator for format checking.
    /// </summary>
    public sealed class EmailAttribute(string? errorMessage = "Invalid e-mail address.") : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null or string { Length: 0 })
                return ValidationResult.Success; // Allow null/empty - use [Required] separately if needed

            if (value is not string email)
                return new ValidationResult(errorMessage);

            if (EmailValidator.IsInvalid(email))
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }
    }
}
