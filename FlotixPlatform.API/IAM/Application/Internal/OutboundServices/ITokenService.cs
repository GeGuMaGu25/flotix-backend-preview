using FlotixPlatform.API.IAM.Domain.Model.Aggregates;

namespace FlotixPlatform.API.IAM.Application.Internal.OutboundServices;

/// <summary>
///     Outbound service contract for issuing and validating bearer tokens.
/// </summary>
public interface ITokenService
{
    /// <summary>
    ///     Generates a time-limited bearer token for a user.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>The bearer token.</returns>
    string GenerateToken(User user);

    /// <summary>
    ///     Validates a bearer token.
    /// </summary>
    /// <param name="token">The bearer token.</param>
    /// <returns>The identifier of the token owner, or null when the token is tampered or expired.</returns>
    Guid? ValidateToken(string token);
}
