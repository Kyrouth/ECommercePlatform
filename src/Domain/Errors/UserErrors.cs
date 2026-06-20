using Domain.Common;

namespace Domain.Errors;

public static class UserErrors
{
    public const string Prefix = nameof(UserErrors);

    public static readonly Error FirstNameMaxLengthError = Error.Validation($"{Prefix}.{nameof(FirstNameMaxLengthError)}", "The first name can not be over than 50 characters");
    public static readonly Error LastNameMaxLengthError = Error.Validation($"{Prefix}.{nameof(LastNameMaxLengthError)}", "The last name can not be over than 50 characters");
}