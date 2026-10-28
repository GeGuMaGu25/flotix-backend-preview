using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Commands;

/// <summary>
///     Command to sign up a user
/// </summary>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The plain password of the user, hashed before persistence.</param>
/// <param name="Role">The role of the user.</param>
/// <param name="FullName">The full name of the user.</param>
/// <param name="LicenseNumber">The driver license number (required for drivers).</param>
/// <param name="LicenseExpiry">The expiry date of the driver license.</param>
/// <param name="CompanyName">The company name of a fleet owner.</param>
/// <param name="WorkshopName">The workshop name of a mechanic.</param>
public record SignUpCommand(EmailAddress Email, string Password, Role Role, string FullName, string? LicenseNumber,
    DateOnly? LicenseExpiry, string? CompanyName, string? WorkshopName);
