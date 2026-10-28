using System.Security.Claims;
using System.Text;
using FlotixPlatform.API.IAM.Application.Internal.OutboundServices;
using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FlotixPlatform.API.IAM.Infrastructure.Tokens.JWT.Services;

/// <summary>
///     JWT bearer token service of the Auth Service.
/// </summary>
/// <remarks>
///     Issues HMAC SHA-256 signed tokens whose subject is the user identifier and whose role claim is the
///     user role type, and validates their signature, issuer, audience and lifetime.
/// </remarks>
/// <param name="tokenSettings">The JWT token settings.</param>
public class TokenService(IOptions<TokenSettings> tokenSettings) : ITokenService
{
    private readonly TokenSettings _settings = tokenSettings.Value;

    private SymmetricSecurityKey SigningKey => new(Encoding.UTF8.GetBytes(_settings.Secret));

    /// <inheritdoc />
    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("role", user.RoleType.ToString())
            ]),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = DateTime.UtcNow.AddHours(_settings.ExpirationHours),
            SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)
        };
        return new JsonWebTokenHandler().CreateToken(tokenDescriptor);
    }

    /// <inheritdoc />
    public async Task<Guid?> ValidateTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        var result = await new JsonWebTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SigningKey,
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        });

        return result.IsValid &&
               result.ClaimsIdentity.FindFirst(JwtRegisteredClaimNames.Sub)?.Value is { } subject &&
               Guid.TryParse(subject, out var userId)
            ? userId
            : null;
    }
}
