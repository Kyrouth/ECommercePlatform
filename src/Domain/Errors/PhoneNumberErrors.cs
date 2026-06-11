using Domain.Common;
using Domain.Enums;

namespace Domain.Errors;


public static class PhoneNumberErrors
{
    public const string Prefix = "PhoneNumber";
    public static readonly Error CharacterLengthError = Error.Failure($"{Prefix}.{nameof(CharacterLengthError)}", $"Phone number must be {ValueObjects.PhoneNumber.CharacterLength} characters.");
    public static readonly Error ComplexityError = Error.Failure($"{Prefix}.{nameof(ComplexityError)}", "Phone number must start with 09 and contain 11 digits.");
}
