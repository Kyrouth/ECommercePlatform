using Domain.Common;

namespace Domain.Entities;


public sealed class RolePermission : BaseEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}