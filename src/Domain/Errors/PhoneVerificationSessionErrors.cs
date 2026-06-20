using Domain.Common;

namespace Domain.Errors;

public static class PhoneVerificationSessionErrors
{
    public const string Prefix = nameof(PhoneVerificationSessionErrors);

    public static readonly Error IncorrectSessionStatusToRefreshError = Error.Conflict($"{Prefix}.{nameof(IncorrectSessionStatusToRefreshError)}", "Cannot refresh a none pending otpSession.");
    public static readonly Error ExpiredSessionError = Error.Conflict($"{Prefix}.{nameof(ExpiredSessionError)}", "Cannot refresh an expired session.");
    public static readonly Error OtpIsNotExpiredError = Error.Conflict($"{Prefix}.{nameof(OtpIsNotExpiredError)}", "Cannot refresh a session that have unexpired otp.");
    public static readonly Error SameOtpError = Error.Conflict($"{Prefix}.{nameof(SameOtpError)}", "Cannot refresh a session with same otp.");
    public static readonly Error Inexpirable = Error.Conflict($"{Prefix}.{nameof(Inexpirable)}", "Cannot expire a session that still have some time.");
    public static readonly Error IncorrectOtpInputError = Error.Validation($"{Prefix}.{nameof(IncorrectOtpInputError)}", "The otp is not correct.");
    public static readonly Error OtpExpiredError = Error.Conflict($"{Prefix}.{nameof(OtpExpiredError)}", "The entered otp is no longer valid, please resend otp.");

}