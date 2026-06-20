using Domain.Common;
using Domain.Errors;

namespace Domain.Entities;

public sealed class Role : BaseEntity
{
    private readonly List<Guid> _permissionIds = new();

    private Role() { Name = null!; }
    private Role(
        Guid id,
        string name,
        bool isSystemRole,
        string? description = null
    )
    {
        Id = id;
        Name = name;
        IsSystemRole = isSystemRole;
        Description = description;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsSystemRole { get; private set; }
    public IReadOnlyCollection<Guid> PermissionIds => _permissionIds;

    public static Result<Role> Create(
        Guid id,
        string name,
        string? description = null,
        bool isSystemRole = false
    )
    {
        if (string.IsNullOrWhiteSpace(name))
            return Error.NullValue;

        if (name.Length > 20)
            return RoleErrors.NameMaximumLengthError;

        if (description is not null && description.Length > 200)
            return RoleErrors.DescriptionMaximumLengthError;

        return new Role(id, name, isSystemRole, description);
    }

    public Result AddPermission(Guid permissionId)
    {
        if (_permissionIds.Contains(permissionId))
            return RoleErrors.PermissionAlreadyExistsError;

        _permissionIds.Add(permissionId);
        return Result.Success();
    }

    public Result RemovePermission(Guid permissionId)
    {
        if(!_permissionIds.Contains(permissionId))
            return RoleErrors.PermissionNotFoundError;

        _permissionIds.Remove(permissionId);

        return Result.Success();
    }
}