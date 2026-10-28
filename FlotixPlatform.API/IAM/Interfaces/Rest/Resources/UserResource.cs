using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a user; the password hash is never exposed.
/// </summary>
/// <param name="Id">The server-generated ID of the user</param>
/// <param name="Email">The email address of the user</param>
/// <param name="RoleType">The role of the user</param>
/// <param name="Name">The full name of the user</param>
/// <param name="LicenseNumber">The driver license number</param>
/// <param name="LicenseExpiration">The expiration date of the driver license</param>
/// <param name="CompanyName">The company name of a fleet owner</param>
/// <param name="WorkshopName">The workshop name of a mechanic</param>
public record UserResource(
    Guid Id,
    string Email,
    Role RoleType,
    string Name,
    string? LicenseNumber,
    DateOnly? LicenseExpiration,
    string? CompanyName,
    string? WorkshopName);
