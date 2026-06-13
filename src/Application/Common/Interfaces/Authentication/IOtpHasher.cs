namespace Application.Common.Interfaces.Authentication;

public interface IOtpHasher
{
    string Hash(string otp, Guid clientId);
}