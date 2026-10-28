using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update the profile of a user.
/// </summary>
/// <param name="Name">The full name of the user (required)</param>
/// <param name="LicenseNumber">The driver license number (required for drivers)</param>
/// <param name="LicenseExpiration">The expiration date of the driver license (required for drivers)</param>
/// <param name="CompanyName">The company name of a fleet owner</param>
/// <param name="WorkshopName">The workshop name of a mechanic</param>
public record UpdateUserProfileResource(
    [Required(ErrorMessage = "RequiredName")]
    [StringLength(100, ErrorMessage = "StringLengthField")]
    string Name,
    [StringLength(50, ErrorMessage = "StringLengthField")]
    string? LicenseNumber,
    DateOnly? LicenseExpiration,
    [StringLength(100, ErrorMessage = "StringLengthField")]
    string? CompanyName,
    [StringLength(100, ErrorMessage = "StringLengthField")]
    string? WorkshopName);
