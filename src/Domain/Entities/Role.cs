using Domain.Common;

namespace Domain.Entities;

public sealed class Role : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
}