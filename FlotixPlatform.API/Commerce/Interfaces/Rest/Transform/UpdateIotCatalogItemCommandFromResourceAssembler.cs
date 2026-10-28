using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateIotCatalogItemCommand from an UpdateIotCatalogItemResource.
/// </summary>
public static class UpdateIotCatalogItemCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateIotCatalogItemCommand from an UpdateIotCatalogItemResource.
    /// </summary>
    /// <param name="iotCatalogItemId">The identifier of the IoT catalog item to update</param>
    /// <param name="resource">The UpdateIotCatalogItemResource resource</param>
    /// <returns>
    ///     An UpdateIotCatalogItemCommand assembled from the UpdateIotCatalogItemResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateIotCatalogItemCommand ToCommandFromResource(int iotCatalogItemId, UpdateIotCatalogItemResource resource) =>
        new(iotCatalogItemId, resource.Name, resource.UnitPrice, resource.IconName);
}
