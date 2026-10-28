using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a UserResource from a User.
/// </summary>
public static class UserResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a UserResource from a User, flattening the data of its specialization.
    /// </summary>
    /// <param name="entity">The User entity (Owner, Driver or Mechanic)</param>
    /// <returns>
    ///     A UserResource assembled from the User
    /// </returns>
    public static UserResource ToResourceFromEntity(User entity) =>
        new(entity.Id, entity.Email.Value, entity.RoleType, entity.Name,
            (entity as Driver)?.LicenseNumber, (entity as Driver)?.LicenseExpiration,
            (entity as Owner)?.CompanyName, (entity as Mechanic)?.WorkshopName);
}
