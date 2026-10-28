using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to sign up a user.
/// </summary>
/// <param name="Email">The email address of the user (required)</param>
/// <param name="Password">The password of the user, at least 8 characters (required)</param>
/// <param name="Role">The role of the user: Owner, Driver or Mechanic</param>
/// <param name="FullName">The full name of the user (required)</param>
/// <param name="LicenseNumber">The driver license number (required for drivers)</param>
/// <param name="LicenseExpiry">The expiry date of the driver license</param>
/// <param name="CompanyName">The company name of a fleet owner</param>
/// <param name="WorkshopName">The workshop name of a mechanic</param>
public record SignUpResource(
    [Required(ErrorMessage = "RequiredEmail")]
    [EmailAddress(ErrorMessage = "EmailAddressField")]
    [StringLength(254, ErrorMessage = "StringLengthField")]
    string Email,
    [Required(ErrorMessage = "RequiredPassword")]
    [MinLength(8, ErrorMessage = "MinLengthField")]
    [StringLength(128, ErrorMessage = "StringLengthField")]
    string Password,
    Role Role,
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
