using Domain.Common;
using Domain.Errors;

namespace Domain.Entities;

public sealed class Permission : BaseEntity
{
    private Permission() { Name = null!; Code = null!; }

    private Permission(
        Guid id,
        string code,
        string name,
        string? description = null
    )
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    public static Result<Permission> Create(
        Guid id,
        string code,
        string name,
        string? description = null
    )
    {
        if(code is null || name is null)
            return Error.NullValue;

        if(code.Length > 30)
            return PermissionErrors.CodeMaximumLengthError;

        if(name.Length > 30)
            return PermissionErrors.NameMaximumLengthError;

        if(description is not null && description.Length > 200)
            return PermissionErrors.DescriptionMaximumLengthError;

        return new Permission(id, code, name, description);
    }
}