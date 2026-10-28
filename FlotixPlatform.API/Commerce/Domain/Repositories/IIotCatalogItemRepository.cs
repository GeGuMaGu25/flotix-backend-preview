using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Commerce.Domain.Repositories;

/// <summary>
///     The IoT catalog item repository contract.
/// </summary>
public interface IIotCatalogItemRepository : IBaseRepository<IotCatalogItem>
{
    /// <summary>
    ///     Check whether an IoT catalog item with the given SKU exists
    /// </summary>
    /// <param name="sku">The SKU to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     True if an IoT catalog item with the SKU exists, false otherwise.
    /// </returns>
    Task<bool> ExistsBySkuAsync(Sku sku, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find an IoT catalog item by SKU
    /// </summary>
    /// <param name="sku">The SKU to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The IoT catalog item object if found, or null otherwise.
    /// </returns>
    Task<IotCatalogItem?> FindBySkuAsync(Sku sku, CancellationToken cancellationToken = default);
}
