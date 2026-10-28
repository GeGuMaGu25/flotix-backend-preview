using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Aggregates;

/// <summary>
///     Represents a fleet owner: manages vehicles and drivers, approves budgets and purchases IoT hardware.
/// </summary>
public class Owner : User
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Owner()
    {
    }

    /// <summary>
    ///     Creates a new Owner from a sign-up command.
    /// </summary>
    /// <param name="command">The SignUpCommand command</param>
    /// <param name="passwordHash">The hash of the user password.</param>
    public Owner(SignUpCommand command, string passwordHash) : base(command, passwordHash, Role.Owner)
    {
        ApplyRoleProfile(command.LicenseNumber, command.LicenseExpiration, command.CompanyName, command.WorkshopName);
    }

    /// <summary>
    ///     Gets the company name of the fleet owner.
    /// </summary>
    public string? CompanyName { get; private set; }

    /// <inheritdoc />
    protected sealed override void ApplyRoleProfile(string? licenseNumber, DateOnly? licenseExpiration,
        string? companyName, string? workshopName)
    {
        CompanyName = string.IsNullOrWhiteSpace(companyName) ? null : companyName.Trim();
    }
}
