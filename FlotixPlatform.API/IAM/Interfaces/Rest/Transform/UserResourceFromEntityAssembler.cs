using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a UserResource from a User.
/// </summary>
public static class UserResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a UserResource from a User.
    /// </summary>
    /// <param name="entity">The User entity</param>
    /// <returns>
    ///     A UserResource assembled from the User
    /// </returns>
    public static UserResource ToResourceFromEntity(User entity) =>
        new(entity.Id, entity.Email.Value, entity.Role, entity.FullName, entity.LicenseNumber, entity.LicenseExpiry,
            entity.CompanyName, entity.WorkshopName);
}
