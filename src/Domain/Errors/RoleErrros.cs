using Domain.Common;

namespace Domain.Errors;

public static class RoleErrors
{
    public const string Prefix = nameof(RoleErrors);

    public static readonly Error NameMaximumLengthError = Error.Validation($"{Prefix}.{nameof(NameMaximumLengthError)}", "The name can not be over than 20 characters");
    public static readonly Error DescriptionMaximumLengthError = Error.Validation($"{Prefix}.{nameof(DescriptionMaximumLengthError)}", "The description can not be over than 200 characters");
    public static readonly Error PermissionAlreadyExistsError = Error.Conflict($"{Prefix}.{nameof(PermissionAlreadyExistsError)}", "The permission is already exists");
    public static readonly Error PermissionNotFoundError = Error.NotFound($"{Prefix}.{nameof(PermissionNotFoundError)}", "The permission is not exists");
}