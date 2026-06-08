using Domain.Common;

namespace Domain.Entities;


public class RefreshToken : BaseEntity
{
    public required string HashToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid DeviceId { get; set; }
    public Guid UserId { get; set; } 
}