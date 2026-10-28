using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateUserProfileCommand from an UpdateUserProfileResource.
/// </summary>
public static class UpdateUserProfileCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateUserProfileCommand from an UpdateUserProfileResource.
    /// </summary>
    /// <param name="userId">The identifier of the user to update</param>
    /// <param name="resource">The UpdateUserProfileResource resource</param>
    /// <returns>
    ///     An UpdateUserProfileCommand assembled from the UpdateUserProfileResource
    /// </returns>
    public static UpdateUserProfileCommand ToCommandFromResource(Guid userId, UpdateUserProfileResource resource) =>
        new(userId, resource.Name, resource.LicenseNumber, resource.LicenseExpiration, resource.CompanyName,
            resource.WorkshopName);
}
