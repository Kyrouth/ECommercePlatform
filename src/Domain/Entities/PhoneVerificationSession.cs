using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;


public sealed class PhoneVerificationSession : BaseEntity
{
    private PhoneVerificationSession()
    {
        OtpHash = null!;
        PhoneNumber = null!;
    }

    private PhoneVerificationSession(
        PhoneNumber phoneNumber,
        string otpHash,
        DateTime expiresAt,
        OtpSessionStatus status,
        Guid deviceId
    )
    {
        PhoneNumber = phoneNumber;
        OtpHash = otpHash;
        ExpiresAt = expiresAt;
        Status = status;
        DeviceId = deviceId;
    }

    public PhoneNumber PhoneNumber { get; private set; }

    public string OtpHash { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public OtpSessionStatus Status { get; private set; }

    public DateTime? VerifiedAt { get; private set; }

    public Guid DeviceId { get; private set; }

    public static PhoneVerificationSession Create(
        PhoneNumber phoneNumber,
        string otpHash,
        DateTime expiresAt,
        Guid deviceId
    )
    {
        return new PhoneVerificationSession(phoneNumber, otpHash, expiresAt, OtpSessionStatus.Pending, deviceId);
    }



}