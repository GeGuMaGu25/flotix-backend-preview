using System.Security.Claims;
using System.Security.Cryptography;
using Flotix.Api.Iam.Domain;
using Microsoft.AspNetCore.DataProtection;

namespace Flotix.Api.Iam.Security;

/// <summary>
/// Opaque, encrypted and time-limited bearer tokens built on ASP.NET Core Data Protection,
/// so the IAM needs no third-party dependency. In production persist the key ring
/// (e.g. <c>AddDataProtection().PersistKeysToFileSystem(...)</c>) so tokens survive restarts and replicas.
/// </summary>
public sealed class DataProtectionTokenService(IDataProtectionProvider provider) : ITokenService
{
    private static readonly TimeSpan Lifetime = TimeSpan.FromHours(8);

    private readonly ITimeLimitedDataProtector _protector = provider.CreateProtector("Flotix.Iam.Token").ToTimeLimitedDataProtector();

    public string Issue(User user) => _protector.Protect($"{user.Id}|{user.Role}", Lifetime);

    public ClaimsPrincipal? Validate(string? token)
    {
        if (string.IsNullOrEmpty(token)) return null;
        try
        {
            return _protector.Unprotect(token).Split('|') is [var id, var role]
                ? new ClaimsPrincipal(new ClaimsIdentity([new(ClaimTypes.NameIdentifier, id), new(ClaimTypes.Role, role)], "Bearer"))
                : null;
        }
        catch (Exception exception) when (exception is CryptographicException or FormatException)
        {
            return null;
        }
    }
}
