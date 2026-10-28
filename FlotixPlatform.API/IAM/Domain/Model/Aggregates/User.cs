using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Aggregates;

/// <summary>
///     Represents a user account of the platform with its credentials and role.
/// </summary>
/// <remarks>
///     This abstract aggregate root is specialized by <see cref="Owner"/>, <see cref="Driver"/> and
///     <see cref="Mechanic"/>; all of them are persisted in the <c>users</c> table and told apart by
///     <see cref="RoleType"/>. The email address is unique within the system and the password is only
///     stored as a hash. Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public abstract partial class User
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected User()
    {
        Email = null!;
        PasswordHash = string.Empty;
        Name = string.Empty;
    }

    /// <summary>
    ///     Initializes the common data of a user from a sign-up command.
    /// </summary>
    /// <param name="command">The SignUpCommand command</param>
    /// <param name="passwordHash">The hash of the user password.</param>
    /// <param name="roleType">The role of the specialization being created.</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    protected User(SignUpCommand command, string passwordHash, Role roleType)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);

        Email = command.Email;
        PasswordHash = passwordHash;
        RoleType = roleType;
        Name = command.Name.Trim();
    }

    /// <summary>
    ///     Gets the server-generated identifier for this user.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the full name of the user.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Gets the email address of the user.
    /// </summary>
    public EmailAddress Email { get; private set; }

    /// <summary>
    ///     Gets the hash of the user password.
    /// </summary>
    public string PasswordHash { get; private set; }

    /// <summary>
    ///     Gets the role of the user, which also identifies its specialization.
    /// </summary>
    public Role RoleType { get; private set; }

    /// <summary>
    ///     Creates the user specialization that corresponds to the role of the sign-up command (Factory Method).
    /// </summary>
    /// <param name="command">The SignUpCommand command</param>
    /// <param name="passwordHash">The hash of the user password.</param>
    /// <returns>An <see cref="Owner"/>, a <see cref="Driver"/> or a <see cref="Mechanic"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a driver has no license number or expiration date.</exception>
    public static User Create(SignUpCommand command, string passwordHash)
    {
        ArgumentNullException.ThrowIfNull(command);
        return command.RoleType switch
        {
            Role.Owner => new Owner(command, passwordHash),
            Role.Driver => new Driver(command, passwordHash),
            Role.Mechanic => new Mechanic(command, passwordHash),
            _ => throw new ArgumentOutOfRangeException(nameof(command), "The role type is not supported.")
        };
    }

    /// <summary>
    ///     Updates the user profile from an update command.
    /// </summary>
    /// <param name="command">The UpdateUserProfileCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a driver has no license number or expiration date.</exception>
    public void UpdateProfile(UpdateUserProfileCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Name);

        ApplyRoleProfile(command.LicenseNumber, command.LicenseExpiration, command.CompanyName, command.WorkshopName);
        Name = command.Name.Trim();
    }

    /// <summary>
    ///     Applies the profile data that belongs to the specialization; data of other roles is ignored.
    /// </summary>
    /// <param name="licenseNumber">The driver license number.</param>
    /// <param name="licenseExpiration">The expiration date of the driver license.</param>
    /// <param name="companyName">The company name of a fleet owner.</param>
    /// <param name="workshopName">The workshop name of a mechanic.</param>
    protected abstract void ApplyRoleProfile(string? licenseNumber, DateOnly? licenseExpiration, string? companyName,
        string? workshopName);
}
