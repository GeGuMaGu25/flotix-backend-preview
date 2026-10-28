namespace FlotixPlatform.API.IAM.Infrastructure.Tokens.JWT.Configuration;

/// <summary>
///     Settings used to issue and validate the JWT bearer tokens of the platform.
/// </summary>
/// <remarks>
///     Bound from the <c>TokenSettings</c> configuration section. In production the secret must be provided
///     through the <c>TokenSettings__Secret</c> environment variable and be at least 32 bytes long.
/// </remarks>
public class TokenSettings
{
    /// <summary>
    ///     Gets or sets the symmetric secret used to sign the tokens (HMAC SHA-256).
    /// </summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the issuer of the tokens.
    /// </summary>
    public string Issuer { get; set; } = "FlotixPlatform.API";

    /// <summary>
    ///     Gets or sets the audience of the tokens.
    /// </summary>
    public string Audience { get; set; } = "FlotixPlatform.WebApplication";

    /// <summary>
    ///     Gets or sets the lifetime of the tokens in hours.
    /// </summary>
    public int ExpirationHours { get; set; } = 8;
}
