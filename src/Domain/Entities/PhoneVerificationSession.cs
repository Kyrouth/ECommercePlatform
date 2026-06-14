using Domain.Common;
using Domain.Enums;
using Domain.Errors;
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
        Guid id,
        PhoneNumber phoneNumber,
        string otpHash,
        DateTime expiresAt,
        DateTime otpExpiresAt,
        OtpSessionStatus status,
        Guid deviceId
    )
    {
        Id = id;
        PhoneNumber = phoneNumber;
        OtpHash = otpHash;
        ExpiresAt = expiresAt;
        Status = status;
        DeviceId = deviceId;
        OtpExpiresAt = otpExpiresAt;
    }

    public PhoneNumber PhoneNumber { get; private set; }

    public string OtpHash { get; private set; }

    public DateTime OtpExpiresAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public OtpSessionStatus Status { get; private set; }

    public DateTime? VerifiedAt { get; private set; }

    public Guid DeviceId { get; private set; }

    public static PhoneVerificationSession Create(
        PhoneNumber phoneNumber,
        string otpHash,
        DateTime now,
        Guid deviceId
    )
    {
        return new PhoneVerificationSession(Guid.NewGuid(), phoneNumber, otpHash, now.AddMinutes(10), now.AddMinutes(3), OtpSessionStatus.Pending, deviceId);
    }

    public Result Refresh(string newOtpHash, DateTime now)
    {
        if (Status != OtpSessionStatus.Pending || VerifiedAt is not null)
            return PhoneVerificationSessionErrors.IncorrectSessionStatusToRefreshError;

        if (ExpiresAt <= now)
        {
            var result = Expire(now);
            if(result.IsFailure)
                return PhoneVerificationSessionErrors.FailedExpireProcessError;
            return PhoneVerificationSessionErrors.ExpiredSessionError;
        }

        if (OtpExpiresAt > now)
            return PhoneVerificationSessionErrors.OtpIsNotExpiredError;

        if (newOtpHash == OtpHash)
            return PhoneVerificationSessionErrors.SameOtpError;



        OtpHash = newOtpHash;
        ExpiresAt = now.AddMinutes(10);
        OtpExpiresAt = now.AddMinutes(3);

        return Result.Success();
    }

    private Result Expire(DateTime now)
    {
        if (Status != OtpSessionStatus.Pending || VerifiedAt is not null)
            return PhoneVerificationSessionErrors.IncorrectSessionStatusToRefreshError;

        if(ExpiresAt > now)
            return PhoneVerificationSessionErrors.Inexpirable;

        Status = OtpSessionStatus.Expired;

        return Result.Success();

    }

    public Result<DateTime> Verify(string otpHash, DateTime now)
    {
        if (Status != OtpSessionStatus.Pending || VerifiedAt is not null)
            return PhoneVerificationSessionErrors.IncorrectSessionStatusToRefreshError;

        if(OtpExpiresAt <= now)
            return PhoneVerificationSessionErrors.OtpExpiredError;
        
        if(otpHash != OtpHash)
            return PhoneVerificationSessionErrors.IncorrectOtpInputError;


        Status = OtpSessionStatus.Verified;
        VerifiedAt = now;
        ExpiresAt = now.AddMinutes(10);


        return Result.Success(ExpiresAt);
    }



}