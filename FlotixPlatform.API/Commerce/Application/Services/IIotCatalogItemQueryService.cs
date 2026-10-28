using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.Queries;

namespace FlotixPlatform.API.Commerce.Application.Services;

/// <summary>
///     Query service interface for IoT catalog item operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying IoT catalog items by various filters.
/// </remarks>
public interface IIotCatalogItemQueryService
{
	/// <summary>
	///     Handle the GetAllIotCatalogItemsQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllIotCatalogItemsQuery. It returns all the IoT catalog items.
	/// </remarks>
	/// <param name="query">The GetAllIotCatalogItemsQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the IotCatalogItem objects</returns>
	Task<IEnumerable<IotCatalogItem>> Handle(GetAllIotCatalogItemsQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetIotCatalogItemByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetIotCatalogItemByIdQuery. It returns the IoT catalog item for the given Id.
	/// </remarks>
	/// <param name="query">The GetIotCatalogItemByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The IotCatalogItem object if found, or null otherwise</returns>
	Task<IotCatalogItem?> Handle(GetIotCatalogItemByIdQuery query, CancellationToken cancellationToken = default);
}
