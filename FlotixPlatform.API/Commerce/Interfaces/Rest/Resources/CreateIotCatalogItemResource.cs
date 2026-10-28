using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an IoT catalog item.
/// </summary>
/// <param name="Sku">The stock keeping unit of the IoT product. (required)</param>
/// <param name="Name">The name of the IoT product. (required)</param>
/// <param name="UnitPrice">The unit price of the IoT product.</param>
/// <param name="IconName">The icon used to display the product. (required)</param>
public record CreateIotCatalogItemResource(
    [Required(ErrorMessage = "RequiredSku")]
    [StringLength(40, ErrorMessage = "StringLengthField")]
    string Sku,
    [Required(ErrorMessage = "RequiredName")]
    [StringLength(120, ErrorMessage = "StringLengthField")]
    string Name,
    [Range(0.01, 100000, ErrorMessage = "RangeField")]
    decimal UnitPrice,
    [Required(ErrorMessage = "RequiredIconName")]
    [StringLength(40, ErrorMessage = "StringLengthField")]
    string IconName);
