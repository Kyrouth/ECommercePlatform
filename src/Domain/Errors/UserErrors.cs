using Domain.Common;

namespace Domain.Errors;

public static partial class UserErrors
{
    public const string Prefix = "User";

    public static readonly Error FirstNameMaxLengthError = Error.Failure($"{Prefix}.{nameof(FirstNameMaxLengthError)}", "The first name can not be over than 50 characters");
    public static readonly Error LastNameMaxLengthError = Error.Failure($"{Prefix}.{nameof(LastNameMaxLengthError)}", "The last name can not be over than 50 characters");
}