using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateWorkshopCommand from an UpdateWorkshopResource.
/// </summary>
public static class UpdateWorkshopCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateWorkshopCommand from an UpdateWorkshopResource.
    /// </summary>
    /// <param name="workshopId">The identifier of the workshop to update</param>
    /// <param name="resource">The UpdateWorkshopResource resource</param>
    /// <returns>
    ///     An UpdateWorkshopCommand assembled from the UpdateWorkshopResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateWorkshopCommand ToCommandFromResource(int workshopId, UpdateWorkshopResource resource) =>
        new(workshopId, resource.City, resource.District, resource.Address, new PhoneNumber(resource.Phone), resource.Rating, resource.Tags ?? []);
}
