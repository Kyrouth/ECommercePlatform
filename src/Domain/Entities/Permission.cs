using Domain.Common;

namespace Domain.Entities;

public sealed class Permission : BaseEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}