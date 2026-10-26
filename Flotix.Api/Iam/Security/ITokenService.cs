using System.Security.Claims;
using Flotix.Api.Iam.Domain;

namespace Flotix.Api.Iam.Security;

public interface ITokenService
{
    string Issue(User user);

    /// <returns>The authenticated principal, or <c>null</c> when the token is missing, tampered or expired.</returns>
    ClaimsPrincipal? Validate(string? token);
}
