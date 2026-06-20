using Domain.Common;

namespace Domain.Errors;

public static class PermissionErrors
{
    public const string Prefix = nameof(PermissionErrors);

    public static readonly Error NameMaximumLengthError = Error.Validation($"{Prefix}.{nameof(NameMaximumLengthError)}", "The name can not be over than 30 characters");
    public static readonly Error CodeMaximumLengthError = Error.Validation($"{Prefix}.{nameof(CodeMaximumLengthError)}", "The code can not be over than 30 characters");
    public static readonly Error DescriptionMaximumLengthError = Error.Validation($"{Prefix}.{nameof(DescriptionMaximumLengthError)}", "The description can not be over than 200 characters");
}