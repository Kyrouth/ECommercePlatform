using Domain.Common;

namespace Domain.Entities;

public class UserDevice : BaseEntity
{
    public required string DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }
}