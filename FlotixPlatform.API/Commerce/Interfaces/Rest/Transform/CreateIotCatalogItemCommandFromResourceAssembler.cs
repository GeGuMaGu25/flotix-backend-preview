using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateIotCatalogItemCommand from a CreateIotCatalogItemResource.
/// </summary>
public static class CreateIotCatalogItemCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateIotCatalogItemCommand from a CreateIotCatalogItemResource.
    /// </summary>
    /// <param name="resource">The CreateIotCatalogItemResource resource</param>
    /// <returns>
    ///     A CreateIotCatalogItemCommand assembled from the CreateIotCatalogItemResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateIotCatalogItemCommand ToCommandFromResource(CreateIotCatalogItemResource resource) =>
        new(new Sku(resource.Sku), resource.Name, resource.UnitPrice, resource.IconName);
}
