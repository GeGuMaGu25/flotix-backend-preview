using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateWorkshopCommand from a CreateWorkshopResource.
/// </summary>
public static class CreateWorkshopCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateWorkshopCommand from a CreateWorkshopResource.
    /// </summary>
    /// <param name="resource">The CreateWorkshopResource resource</param>
    /// <returns>
    ///     A CreateWorkshopCommand assembled from the CreateWorkshopResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateWorkshopCommand ToCommandFromResource(CreateWorkshopResource resource) =>
        new(resource.MechanicId, resource.City, resource.District, resource.Address, new PhoneNumber(resource.Phone), resource.Rating, resource.Tags ?? []);
}
