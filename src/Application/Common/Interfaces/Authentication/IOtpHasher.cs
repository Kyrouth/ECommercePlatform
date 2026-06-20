namespace Application.Common.Interfaces.Authentication;

public interface IOtpHasher
{
    string HashOtp(string otp, Guid clientId);
}