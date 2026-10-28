using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a workshop.
/// </summary>
/// <param name="City">The city where the workshop is located. (required)</param>
/// <param name="District">The district where the workshop is located. (required)</param>
/// <param name="Address">The street address of the workshop. (required)</param>
/// <param name="Phone">The contact phone number of the workshop. (required)</param>
/// <param name="Rating">The rating of the workshop, from 0 to 5.</param>
/// <param name="Tags">The services offered by the workshop.</param>
public record UpdateWorkshopResource(
    [Required(ErrorMessage = "RequiredCity")]
    [StringLength(80, ErrorMessage = "StringLengthField")]
    string City,
    [Required(ErrorMessage = "RequiredDistrict")]
    [StringLength(80, ErrorMessage = "StringLengthField")]
    string District,
    [Required(ErrorMessage = "RequiredAddress")]
    [StringLength(200, ErrorMessage = "StringLengthField")]
    string Address,
    [Required(ErrorMessage = "RequiredPhone")]
    [StringLength(30, ErrorMessage = "StringLengthField")]
    string Phone,
    [Range(0, 5, ErrorMessage = "RangeField")]
    decimal Rating,
    [MaxLength(20, ErrorMessage = "MaxLengthField")]
    List<string>? Tags);
