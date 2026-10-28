namespace FlotixPlatform.API.Commerce.Domain.Model.Commands;

/// <summary>
///     Command to update an IoT catalog item
/// </summary>
/// <param name="IotCatalogItemId">The identifier of the IoT catalog item to update</param>
/// <param name="Name">The name of the IoT product.</param>
/// <param name="UnitPrice">The unit price of the IoT product.</param>
/// <param name="IconName">The icon used to display the product.</param>
public record UpdateIotCatalogItemCommand(int IotCatalogItemId, string Name, decimal UnitPrice, string IconName);
