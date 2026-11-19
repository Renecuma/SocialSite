using System.ComponentModel.DataAnnotations;

namespace SocialSite.Application.Validation;

// Vlastní validační atribut: ověří, že datum narození odpovídá alespoň zadanému věku
[AttributeUsage(AttributeTargets.Property)]
public class MinimumAgeAttribute : ValidationAttribute
{
    public int MinimumAge { get; }

    public MinimumAgeAttribute(int minimumAge)
    {
        MinimumAge = minimumAge;
        ErrorMessage = $"Musí ti být alespoň {minimumAge} let.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success; // povinnost řeší [Required]

        if (value is not DateOnly dateOfBirth)
            return new ValidationResult("Neplatné datum narození.");

        var today = DateOnly.FromDateTime(DateTime.Today);
        if (dateOfBirth > today)
            return new ValidationResult("Datum narození nemůže být v budoucnosti.");

        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age)) age--;

        return age >= MinimumAge
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
    }
}
