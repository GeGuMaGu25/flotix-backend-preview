using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents an authenticated user and the bearer token issued for it.
/// </summary>
/// <param name="Id">The server-generated ID of the user</param>
/// <param name="Email">The email address of the user</param>
/// <param name="Role">The role of the user</param>
/// <param name="FullName">The full name of the user</param>
/// <param name="Token">The bearer token to authorize further requests</param>
public record AuthenticatedUserResource(
    int Id,
    string Email,
    Role Role,
    string FullName,
    string Token);
