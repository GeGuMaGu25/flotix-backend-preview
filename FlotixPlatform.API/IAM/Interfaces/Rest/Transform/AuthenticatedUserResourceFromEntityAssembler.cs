using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an AuthenticatedUserResource from a User and its bearer token.
/// </summary>
public static class AuthenticatedUserResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles an AuthenticatedUserResource from a User and its bearer token.
    /// </summary>
    /// <param name="entity">The authenticated User entity</param>
    /// <param name="token">The bearer token issued for the user</param>
    /// <returns>
    ///     An AuthenticatedUserResource assembled from the User and the token
    /// </returns>
    public static AuthenticatedUserResource ToResourceFromEntity(User entity, string token) =>
        new(entity.Id, entity.Email.Value, entity.Role, entity.FullName, token);
}
