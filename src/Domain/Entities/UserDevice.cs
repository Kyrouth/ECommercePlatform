using Domain.Common;

namespace Domain.Entities;

public sealed class UserDevice : BaseEntity
{
    private UserDevice() {}

    private UserDevice(
        Guid deviceId,
        Guid? userId,
        string? deviceName,
        string? userAgent,
        string? ipAddress
    )
    {
        DeviceId = deviceId;
        DeviceName = deviceName;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        UserId = userId;
    }

    public Guid DeviceId { get; private set; }
    public Guid? UserId { get; private set; }
    public string? DeviceName { get; private set; }
    public string? UserAgent { get; private set; }
    public string? IpAddress { get; private set; }

    public static UserDevice Create(
        Guid deviceId,
        Guid? userId = null,
        string? deviceName = null,
        string? userAgent = null,
        string? ipAddress = null
    )
    {
        return new UserDevice(deviceId, userId, deviceName, userAgent, ipAddress);
    }
}