namespace FlotixPlatform.API.IAM.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract for hashing and verifying passwords.
/// </summary>
public interface IHashingService
{
    /// <summary>
    ///     Hashes a plain password.
    /// </summary>
    /// <param name="password">The plain password.</param>
    /// <returns>The password hash to persist.</returns>
    string HashPassword(string password);

    /// <summary>
    ///     Verifies a plain password against a stored hash.
    /// </summary>
    /// <param name="password">The plain password.</param>
    /// <param name="passwordHash">The stored password hash.</param>
    /// <returns>True if the password matches the hash, false otherwise.</returns>
    bool VerifyPassword(string password, string passwordHash);
}
