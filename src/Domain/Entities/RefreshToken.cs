using Domain.Common;

namespace Domain.Entities;


public sealed class RefreshToken : BaseEntity
{
    private RefreshToken(
        Guid id,
        string hashToken,
        DateTime expiresAt,
        Guid deviceId,
        Guid userId
    )
    {
        Id = id;
        HashToken = hashToken;
        ExpiresAt = expiresAt;
        DeviceId = deviceId;
        UserId = userId;
    }
    public string HashToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid UserId { get; private set; }

    public static RefreshToken Create(
        Guid id,
        string hashToken,
        DateTime now,
        Guid deviceId,
        Guid userId
    )
    {
        return new RefreshToken(id, hashToken, now.AddDays(30), deviceId, userId);
    }


}