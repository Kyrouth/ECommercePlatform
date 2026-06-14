using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enums;
using Domain.Errors;

namespace Domain.UnitTests.Entities;

public sealed class PhoneVerificationSessionTests
{
    private static PhoneVerificationSession CreateSession(
        DateTime now,
        string otpHash = "hash1")
    {
        var phoneNumber = PhoneNumber.Create("09123456789").Value;

        return PhoneVerificationSession.Create(
            phoneNumber,
            otpHash,
            now,
            Guid.NewGuid()
        );
    }

    // -------------------------
    // CREATE
    // -------------------------

    [Fact]
    public void Create_ShouldInitializeCorrectState()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now);

        Assert.Equal(OtpSessionStatus.Pending, session.Status);
        Assert.Null(session.VerifiedAt);
        Assert.Equal("hash1", session.OtpHash);
    }

    // -------------------------
    // VERIFY
    // -------------------------

    [Fact]
    public void Verify_WithCorrectOtp_ShouldReturnSuccess()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now);

        var result = session.Verify("hash1", now.AddMinutes(1));

        Assert.True(result.IsSuccess);
        Assert.Equal(OtpSessionStatus.Verified, session.Status);
        Assert.NotNull(session.VerifiedAt);
    }

    [Fact]
    public void Verify_WithWrongOtp_ShouldFail()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now);

        var result = session.Verify("wrong_hash", now.AddMinutes(1));

        Assert.True(result.IsFailure);
        Assert.Equal(PhoneVerificationSessionErrors.IncorrectOtpInputError, result.Error);
    }

    [Fact]
    public void Verify_WhenOtpExpired_ShouldFail()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now);

        var result = session.Verify("hash1", now.AddMinutes(5)); 
        // otp expires at +3 min

        Assert.True(result.IsFailure);
        Assert.Equal(PhoneVerificationSessionErrors.OtpExpiredError, result.Error);
    }

    [Fact]
    public void Verify_WhenAlreadyVerified_ShouldFail()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now);

        session.Verify("hash1", now.AddMinutes(1));

        var result = session.Verify("hash1", now.AddMinutes(2));

        Assert.True(result.IsFailure);
        Assert.Equal(PhoneVerificationSessionErrors.IncorrectSessionStatusToRefreshError, result.Error);
    }

    // -------------------------
    // REFRESH
    // -------------------------

    [Fact]
    public void Refresh_WithValidState_ShouldUpdateOtp()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now, "old_hash");

        var result = session.Refresh("new_hash", now.AddMinutes(4)); 
        // otp expired after 3 min

        Assert.True(result.IsSuccess);
        Assert.Equal("new_hash", session.OtpHash);
    }

    [Fact]
    public void Refresh_WhenOtpNotExpired_ShouldFail()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now, "hash1");

        var result = session.Refresh("new_hash", now.AddMinutes(1));

        Assert.True(result.IsFailure);
        Assert.Equal(PhoneVerificationSessionErrors.OtpIsNotExpiredError, result.Error);
    }

    [Fact]
    public void Refresh_WhenSameOtp_ShouldFail()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now, "hash1");

        var result = session.Refresh("hash1", now.AddMinutes(5));

        Assert.True(result.IsFailure);
        Assert.Equal(PhoneVerificationSessionErrors.SameOtpError, result.Error);
    }

    [Fact]
    public void Refresh_WhenExpiredSession_ShouldExpireAndFail()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now, "hash1");

        var result = session.Refresh("new_hash", now.AddMinutes(15));

        Assert.True(result.IsFailure);
        Assert.Equal(PhoneVerificationSessionErrors.ExpiredSessionError, result.Error);
        Assert.Equal(OtpSessionStatus.Expired, session.Status);
    }

    // -------------------------
    // EXPIRE (indirect via refresh)
    // -------------------------

    [Fact]
    public void Expire_ShouldSetStatusToExpired()
    {
        var now = DateTime.UtcNow;

        var session = CreateSession(now, "hash1");

        var result = session.Refresh("new_hash", now.AddMinutes(20));

        Assert.True(result.IsFailure);
        Assert.Equal(OtpSessionStatus.Expired, session.Status);
    }
}