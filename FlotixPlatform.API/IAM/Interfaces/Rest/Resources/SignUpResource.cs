using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to sign up a user.
/// </summary>
/// <param name="Email">The email address of the user (required)</param>
/// <param name="Password">The password of the user, at least 8 characters (required)</param>
/// <param name="RoleType">The role of the user: Owner, Driver or Mechanic</param>
/// <param name="Name">The full name of the user (required)</param>
/// <param name="LicenseNumber">The driver license number (required for drivers)</param>
/// <param name="LicenseExpiration">The expiration date of the driver license (required for drivers)</param>
/// <param name="CompanyName">The company name of a fleet owner</param>
/// <param name="WorkshopName">The workshop name of a mechanic</param>
public record SignUpResource(
    [Required(ErrorMessage = "RequiredEmail")]
    [EmailAddress(ErrorMessage = "EmailAddressField")]
    [StringLength(150, ErrorMessage = "StringLengthField")]
    string Email,
    [Required(ErrorMessage = "RequiredPassword")]
    [MinLength(8, ErrorMessage = "MinLengthField")]
    [StringLength(128, ErrorMessage = "StringLengthField")]
    string Password,
    Role RoleType,
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
