using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;


public sealed class PhoneVerificationSession : BaseEntity
{
    public required string PhoneNumber { get; set; }

    public required string OtpHash { get; set; }

    public DateTime ExpiresAt { get; set; }

    public OtpSessionStatus Status { get; set; }

    public DateTime? VerifiedAt { get; set; }
}