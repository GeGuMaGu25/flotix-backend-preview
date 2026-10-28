namespace FlotixPlatform.API.Commerce.Domain.Model.Commands;

/// <summary>
///     Command to delete an IoT catalog item
/// </summary>
/// <param name="IotCatalogItemId">The identifier of the IoT catalog item to delete</param>
public record DeleteIotCatalogItemCommand(int IotCatalogItemId);
