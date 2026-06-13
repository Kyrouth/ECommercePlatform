using System.Security.Cryptography;

namespace Application.Common.Utilities;

public static class OtpGenerator
{
    public static string Generate(int length = 6)
    {
        var max = (int)Math.Pow(10, length);
        return RandomNumberGenerator.GetInt32(0, max)
            .ToString($"D{length}");
    }
}