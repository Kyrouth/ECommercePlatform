using Domain.Common;

namespace Domain.Errors;


public static class UsernameErrors
{
    public const string Prefix = nameof(UsernameErrors);
    public static readonly Error MinAndMaxLengthError = Error.Validation($"{Prefix}.{nameof(MinAndMaxLengthError)}", $"The username must be at least {ValueObjects.Username.MinLength} characters and can not be over than {ValueObjects.Username.MaxLength} characters.");
    public static readonly Error ComplexityError = Error.Validation($"{Prefix}.{nameof(ComplexityError)}", "Username must start with a letter and contain only English letters, numbers, and underscore (_).");
}