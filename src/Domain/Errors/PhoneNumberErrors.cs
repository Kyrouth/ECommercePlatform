using Domain.Common;

namespace Domain.Errors;


public static class PhoneNumberErrors
{
    public const string Prefix = nameof(PhoneNumberErrors);
    public static readonly Error CharacterLengthError = Error.Validation($"{Prefix}.{nameof(CharacterLengthError)}", $"Phone number must be {ValueObjects.PhoneNumber.CharacterLength} characters.");
    public static readonly Error ComplexityError = Error.Validation($"{Prefix}.{nameof(ComplexityError)}", "Phone number must start with 09 and contain 11 digits.");
}
