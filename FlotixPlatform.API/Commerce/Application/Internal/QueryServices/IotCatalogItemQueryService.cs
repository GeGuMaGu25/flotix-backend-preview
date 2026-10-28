using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Model.Queries;
using FlotixPlatform.API.Commerce.Domain.Repositories;

namespace FlotixPlatform.API.Commerce.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying IoT catalog items.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving IoT catalog items.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="iotCatalogItemRepository">Repository for accessing IoT catalog item data.</param>
public class IotCatalogItemQueryService(IIotCatalogItemRepository iotCatalogItemRepository)
    : IIotCatalogItemQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<IotCatalogItem>> Handle(GetAllIotCatalogItemsQuery query,
        CancellationToken cancellationToken = default) =>
        await iotCatalogItemRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IotCatalogItem?> Handle(GetIotCatalogItemByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await iotCatalogItemRepository.FindByIdAsync(query.Id, cancellationToken);
}
