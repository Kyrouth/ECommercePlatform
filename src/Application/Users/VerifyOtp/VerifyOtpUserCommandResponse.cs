namespace Application.Users.VerifyOtp;

public sealed record VerifyOtpUserCommandResponse(
    Guid VerificationTokenId,
    DateTime ExpiresAt
);