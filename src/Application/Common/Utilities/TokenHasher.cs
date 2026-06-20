using System.Security.Cryptography;
using System.Text;

namespace Application.Common.Utilities;


public static class TokenHasher
{
    public static string HashToken(string token)
{
    using var sha256 = SHA256.Create();

    var bytes = Encoding.UTF8.GetBytes(token);
    var hash = sha256.ComputeHash(bytes);

    return Convert.ToBase64String(hash);
}
}