using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Aggregates;

/// <summary>
///     Represents a mechanic or affiliated workshop: diagnoses, quotes and repairs vehicles.
/// </summary>
public class Mechanic : User
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Mechanic()
    {
    }

    /// <summary>
    ///     Creates a new Mechanic from a sign-up command.
    /// </summary>
    /// <param name="command">The SignUpCommand command</param>
    /// <param name="passwordHash">The hash of the user password.</param>
    public Mechanic(SignUpCommand command, string passwordHash) : base(command, passwordHash, Role.Mechanic)
    {
        ApplyRoleProfile(command.LicenseNumber, command.LicenseExpiration, command.CompanyName, command.WorkshopName);
    }

    /// <summary>
    ///     Gets the workshop name of the mechanic.
    /// </summary>
    public string? WorkshopName { get; private set; }

    /// <inheritdoc />
    protected sealed override void ApplyRoleProfile(string? licenseNumber, DateOnly? licenseExpiration,
        string? companyName, string? workshopName)
    {
        WorkshopName = string.IsNullOrWhiteSpace(workshopName) ? null : workshopName.Trim();
    }
}
