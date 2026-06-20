
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces.Authentication;

namespace Infrastructure.Authentication;

public sealed class HmacOtpHasher : IOtpHasher
{
    private readonly string _secret;

    public HmacOtpHasher(string secret)
    {
        _secret = secret;
    }

    public string HashOtp(string otp, Guid clientId)
    {
        using var hmac = new HMACSHA256(
            Encoding.UTF8.GetBytes(_secret));

        var input = $"{clientId}:{otp}";
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));

        return Convert.ToBase64String(hash);
    }
}