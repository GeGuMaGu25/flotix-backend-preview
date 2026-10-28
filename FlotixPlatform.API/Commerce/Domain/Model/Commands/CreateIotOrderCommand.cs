using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Commerce.Domain.Model.Commands;

/// <summary>
///     Command to create an IoT order
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="Sku">The stock keeping unit of the IoT product.</param>
/// <param name="DeviceQuantity">The number of devices ordered.</param>
public record CreateIotOrderCommand(int OwnerId, Sku Sku, int DeviceQuantity);
