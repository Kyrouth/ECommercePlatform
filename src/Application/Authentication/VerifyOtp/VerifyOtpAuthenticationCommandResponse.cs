namespace Application.Authentication.VerifyOtp;

public sealed record VerifyOtpAuthenticationCommandResponse(
    Guid VerificationTokenId,
    DateTime ExpiresAt,
    bool IsExistingUser,
    string? AccessToken = null,
    string? RefreshToken = null
);