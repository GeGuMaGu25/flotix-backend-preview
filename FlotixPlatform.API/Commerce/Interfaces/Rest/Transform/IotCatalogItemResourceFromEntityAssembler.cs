using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a IotCatalogItemResource from a IotCatalogItem.
/// </summary>
public static class IotCatalogItemResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a IotCatalogItemResource from a IotCatalogItem.
    /// </summary>
    /// <param name="entity">The IotCatalogItem entity</param>
    /// <returns>
    ///     A IotCatalogItemResource assembled from the IotCatalogItem
    /// </returns>
    public static IotCatalogItemResource ToResourceFromEntity(IotCatalogItem entity) =>
        new(entity.Id, entity.Sku.Value, entity.Name, entity.UnitPrice, entity.IconName);
}
