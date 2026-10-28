using System.Security.Cryptography;
using FlotixPlatform.API.IAM.Application.Internal.OutboundServices;
using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using Microsoft.AspNetCore.DataProtection;

namespace FlotixPlatform.API.IAM.Infrastructure.Tokens.DataProtection.Services;

/// <summary>
///     Bearer token service based on ASP.NET Core Data Protection.
/// </summary>
/// <remarks>
///     Tokens are opaque, encrypted, authenticated and time-limited, and need no third-party dependency.
///     In production persist the key ring (e.g. <c>AddDataProtection().PersistKeysToFileSystem(...)</c>)
///     so tokens survive restarts and are shared between replicas.
/// </remarks>
/// <param name="dataProtectionProvider">The data protection provider.</param>
public class TokenService(IDataProtectionProvider dataProtectionProvider) : ITokenService
{
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);

    private readonly ITimeLimitedDataProtector _protector = dataProtectionProvider
        .CreateProtector("FlotixPlatform.IAM.Tokens")
        .ToTimeLimitedDataProtector();

    /// <inheritdoc />
    public string GenerateToken(User user) =>
        _protector.Protect(user.Id.ToString(), TokenLifetime);

    /// <inheritdoc />
    public Guid? ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        try
        {
            return Guid.TryParse(_protector.Unprotect(token), out var userId)
                ? userId
                : null;
        }
        catch (Exception ex) when (ex is CryptographicException or FormatException)
        {
            return null;
        }
    }
}
