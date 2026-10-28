namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an IoT catalog item.
/// </summary>
/// <param name="Id">The server-generated ID of the IoT catalog item</param>
/// <param name="Sku">The stock keeping unit of the IoT product.</param>
/// <param name="Name">The name of the IoT product.</param>
/// <param name="UnitPrice">The unit price of the IoT product.</param>
/// <param name="IconName">The icon used to display the product.</param>
public record IotCatalogItemResource(
    int Id,
    string Sku,
    string Name,
    decimal UnitPrice,
    string IconName);
