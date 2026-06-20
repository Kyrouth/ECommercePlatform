using Domain.Common;

namespace Domain.Entities;

public sealed class UserDevice : BaseEntity
{
    private UserDevice() {}

    private UserDevice(
        Guid id,
        Guid clientId,
        Guid? userId,
        string? userAgent,
        string? ipAddress
    )
    {
        Id = id;
        ClientId = clientId;
        UserAgent = userAgent;
        IpAddress = ipAddress;
        UserId = userId;
    }

    public Guid ClientId { get; private set; }
    public Guid? UserId { get; private set; }
    public string? UserAgent { get; private set; }
    public string? IpAddress { get; private set; }

    public static UserDevice Create(
        Guid id,
        Guid clientId,
        Guid? userId = null,
        string? userAgent = null,
        string? ipAddress = null
    )
    {
        return new UserDevice(id, clientId, userId, userAgent, ipAddress);
    }

    public void SetUser(Guid userId)
    {
        UserId = userId;
    }
}