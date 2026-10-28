using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update the profile of a user.
/// </summary>
/// <param name="FullName">The full name of the user (required)</param>
/// <param name="LicenseNumber">The driver license number (required for drivers)</param>
/// <param name="LicenseExpiry">The expiry date of the driver license</param>
/// <param name="CompanyName">The company name of a fleet owner</param>
/// <param name="WorkshopName">The workshop name of a mechanic</param>
public record UpdateUserProfileResource(
    [Required(ErrorMessage = "RequiredFullName")]
    [StringLength(120, ErrorMessage = "StringLengthField")]
    string FullName,
    [StringLength(20, ErrorMessage = "StringLengthField")]
    string? LicenseNumber,
    DateOnly? LicenseExpiry,
    [StringLength(120, ErrorMessage = "StringLengthField")]
    string? CompanyName,
    [StringLength(120, ErrorMessage = "StringLengthField")]
    string? WorkshopName);
