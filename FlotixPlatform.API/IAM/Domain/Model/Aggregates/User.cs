using System.Diagnostics.CodeAnalysis;
using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Aggregates;

/// <summary>
///     Represents a user account of the platform with its security credentials, role and profile.
/// </summary>
/// <remarks>
///     This aggregate root is created by signing up and its profile is updated through an UpdateUserProfileCommand.
///     The email address is unique within the system and the password is only stored as a hash.
///     A driver must always have a license number.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class User
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected User()
    {
        Email = null!;
        PasswordHash = string.Empty;
        FullName = string.Empty;
    }

    /// <summary>
    ///     Creates a new User from a sign-up command.
    /// </summary>
    /// <param name="command">The SignUpCommand command</param>
    /// <param name="passwordHash">The hash of the user password.</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a driver has no license number.</exception>
    public User(SignUpCommand command, string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        Email = command.Email;
        PasswordHash = passwordHash;
        Role = command.Role;
        ApplyProfile(command.FullName, command.LicenseNumber, command.LicenseExpiry, command.CompanyName,
            command.WorkshopName);
    }

    /// <summary>
    ///     Gets the server-generated identifier for this user.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the email address of the user.
    /// </summary>
    public EmailAddress Email { get; private set; }

    /// <summary>
    ///     Gets the hash of the user password.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    ///     Gets the role of the user.
    /// </summary>
    public Role Role { get; private set; }

    /// <summary>
    ///     Gets the full name of the user.
    /// </summary>
    public string FullName { get; private set; }

    /// <summary>
    ///     Gets the driver license number (required for drivers).
    /// </summary>
    public string? LicenseNumber { get; private set; }

    /// <summary>
    ///     Gets the expiry date of the driver license.
    /// </summary>
    public DateOnly? LicenseExpiry { get; private set; }

    /// <summary>
    ///     Gets the company name of a fleet owner.
    /// </summary>
    public string? CompanyName { get; private set; }

    /// <summary>
    ///     Gets the workshop name of a mechanic.
    /// </summary>
    public string? WorkshopName { get; private set; }

    /// <summary>
    ///     Updates the user profile from an update command.
    /// </summary>
    /// <param name="command">The UpdateUserProfileCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a driver has no license number.</exception>
    public void UpdateProfile(UpdateUserProfileCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ApplyProfile(command.FullName, command.LicenseNumber, command.LicenseExpiry, command.CompanyName,
            command.WorkshopName);
    }

    [MemberNotNull(nameof(FullName))]
    private void ApplyProfile(string fullName, string? licenseNumber, DateOnly? licenseExpiry, string? companyName,
        string? workshopName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        if (Role == Role.Driver && string.IsNullOrWhiteSpace(licenseNumber))
            throw new InvalidOperationException("A driver must have a license number.");

        FullName = fullName.Trim();
        LicenseNumber = licenseNumber?.Trim();
        LicenseExpiry = licenseExpiry;
        CompanyName = companyName?.Trim();
        WorkshopName = workshopName?.Trim();
    }
}
