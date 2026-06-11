using Domain.Common;

namespace Domain.Errors;


public static class UsernameErrors
{
    public const string Prefix = "Username";
    public static readonly Error MinAndMaxLengthError = Error.Failure($"{Prefix}.{nameof(MinAndMaxLengthError)}", $"The username must be at least {ValueObjects.Username.MinLength} characters and can not be over than {ValueObjects.Username.MaxLength} characters.");
    public static readonly Error ComplexityError = Error.Failure($"{Prefix}.{nameof(ComplexityError)}", "Username must start with a letter and contain only English letters, numbers, and underscore (_).");
}