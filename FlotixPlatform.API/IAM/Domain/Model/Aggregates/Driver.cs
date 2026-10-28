using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Aggregates;

/// <summary>
///     Represents a driver: operates vehicles, registers refuels and reports incidents.
/// </summary>
/// <remarks>
///     Business policy: the license number and its expiration date are mandatory for a driver.
/// </remarks>
public class Driver : User
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Driver()
    {
    }

    /// <summary>
    ///     Creates a new Driver from a sign-up command.
    /// </summary>
    /// <param name="command">The SignUpCommand command</param>
    /// <param name="passwordHash">The hash of the user password.</param>
    /// <exception cref="InvalidOperationException">Thrown when the license number or its expiration date is missing.</exception>
    public Driver(SignUpCommand command, string passwordHash) : base(command, passwordHash, Role.Driver)
    {
        ApplyRoleProfile(command.LicenseNumber, command.LicenseExpiration, command.CompanyName, command.WorkshopName);
    }

    /// <summary>
    ///     Gets the driver license number.
    /// </summary>
    public string? LicenseNumber { get; private set; }

    /// <summary>
    ///     Gets the expiration date of the driver license.
    /// </summary>
    public DateOnly? LicenseExpiration { get; private set; }

    /// <inheritdoc />
    protected sealed override void ApplyRoleProfile(string? licenseNumber, DateOnly? licenseExpiration,
        string? companyName, string? workshopName)
    {
        if (string.IsNullOrWhiteSpace(licenseNumber) || licenseExpiration is null)
            throw new InvalidOperationException("A driver must have a license number and its expiration date.");

        LicenseNumber = licenseNumber.Trim();
        LicenseExpiration = licenseExpiration;
    }
}
