using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

public sealed class JwtOptions
{
    public string SecretKey { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int TimeoutInMin { get; init; }
}

public sealed class TokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;
    private readonly SigningCredentials _credentials;
    private readonly JwtSecurityTokenHandler _handler = new();

    public TokenProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        var keyBytes = Encoding.UTF8.GetBytes(_options.SecretKey);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        _credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    public string Create(Guid userId)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var jwtToken = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.TimeoutInMin),
            signingCredentials: _credentials
        );

        return _handler.WriteToken(jwtToken);
    }

    public string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}