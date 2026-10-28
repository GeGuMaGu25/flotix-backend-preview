using FlotixPlatform.API.IAM.Application.Internal.OutboundServices;
using Microsoft.AspNetCore.Identity;

namespace FlotixPlatform.API.IAM.Infrastructure.Hashing.AspNetCore.Services;

/// <summary>
///     Password hashing service based on the ASP.NET Core Identity password hasher (PBKDF2).
/// </summary>
/// <remarks>
///     It is part of the shared framework, so the IAM needs no third-party hashing dependency.
/// </remarks>
public class HashingService : IHashingService
{
    private static readonly PasswordHasher<HashingService> PasswordHasher = new();

    /// <inheritdoc />
    public string HashPassword(string password) => PasswordHasher.HashPassword(this, password);

    /// <inheritdoc />
    public bool VerifyPassword(string password, string passwordHash) =>
        PasswordHasher.VerifyHashedPassword(this, passwordHash, password) != PasswordVerificationResult.Failed;
}
