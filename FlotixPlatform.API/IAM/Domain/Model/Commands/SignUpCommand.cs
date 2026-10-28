using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Commands;

/// <summary>
///     Command to sign up a user
/// </summary>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The plain password of the user, hashed before persistence.</param>
/// <param name="RoleType">The role of the user.</param>
/// <param name="Name">The full name of the user.</param>
/// <param name="LicenseNumber">The driver license number (required for drivers).</param>
/// <param name="LicenseExpiration">The expiration date of the driver license (required for drivers).</param>
/// <param name="CompanyName">The company name of a fleet owner.</param>
/// <param name="WorkshopName">The workshop name of a mechanic.</param>
public record SignUpCommand(EmailAddress Email, string Password, Role RoleType, string Name, string? LicenseNumber,
    DateOnly? LicenseExpiration, string? CompanyName, string? WorkshopName);
