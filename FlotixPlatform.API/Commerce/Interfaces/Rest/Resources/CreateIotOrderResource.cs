using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an IoT order.
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="Sku">The stock keeping unit of the IoT product. (required)</param>
/// <param name="DeviceQuantity">The number of devices ordered.</param>
public record CreateIotOrderResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int OwnerId,
    [Required(ErrorMessage = "RequiredSku")]
    [StringLength(40, ErrorMessage = "StringLengthField")]
    string Sku,
    [Range(1, 1000, ErrorMessage = "RangeField")]
    int DeviceQuantity);
