namespace FlotixPlatform.API.IAM.Domain.Model.Commands;

/// <summary>
///     Command to update the profile of a user
/// </summary>
/// <param name="UserId">The identifier of the user to update</param>
/// <param name="Name">The full name of the user.</param>
/// <param name="LicenseNumber">The driver license number (required for drivers).</param>
/// <param name="LicenseExpiration">The expiration date of the driver license (required for drivers).</param>
/// <param name="CompanyName">The company name of a fleet owner.</param>
/// <param name="WorkshopName">The workshop name of a mechanic.</param>
public record UpdateUserProfileCommand(Guid UserId, string Name, string? LicenseNumber, DateOnly? LicenseExpiration,
    string? CompanyName, string? WorkshopName);
