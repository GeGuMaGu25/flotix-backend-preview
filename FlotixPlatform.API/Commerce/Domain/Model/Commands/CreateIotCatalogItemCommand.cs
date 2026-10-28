using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Commerce.Domain.Model.Commands;

/// <summary>
///     Command to create an IoT catalog item
/// </summary>
/// <param name="Sku">The stock keeping unit of the IoT product.</param>
/// <param name="Name">The name of the IoT product.</param>
/// <param name="UnitPrice">The unit price of the IoT product.</param>
/// <param name="IconName">The icon used to display the product.</param>
public record CreateIotCatalogItemCommand(Sku Sku, string Name, decimal UnitPrice, string IconName);
