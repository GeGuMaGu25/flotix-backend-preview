namespace FlotixPlatform.API.Commerce.Domain.Model.Queries;

/// <summary>
///     Query to get an IoT catalog item by ID
/// </summary>
/// <param name="Id">The IoT catalog item ID to search</param>
public record GetIotCatalogItemByIdQuery(int Id);
