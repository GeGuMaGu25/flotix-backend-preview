using FlotixPlatform.API.Notifications.Domain.Model.Commands;

namespace FlotixPlatform.API.Notifications.Domain.Model.Aggregates;

/// <summary>
///     Represents the kinds of notification a user wants to receive.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateNotificationPreferenceCommand and updated through an UpdateNotificationPreferenceCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class NotificationPreference
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected NotificationPreference()
    {
    }

    /// <summary>
    ///     Creates a new NotificationPreference from a creation command.
    /// </summary>
    /// <param name="command">The CreateNotificationPreferenceCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public NotificationPreference(CreateNotificationPreferenceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.UserId, 1);

        UserId = command.UserId;
        Maintenance = command.Maintenance;
        Speed = command.Speed;
        Incidents = command.Incidents;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this notification preference.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the user.
    /// </summary>
    public int UserId { get; private set; }

    /// <summary>
    ///     Gets whether maintenance notifications are enabled.
    /// </summary>
    public bool Maintenance { get; private set; }

    /// <summary>
    ///     Gets whether speeding notifications are enabled.
    /// </summary>
    public bool Speed { get; private set; }

    /// <summary>
    ///     Gets whether incident notifications are enabled.
    /// </summary>
    public bool Incidents { get; private set; }

    /// <summary>
    ///     Updates the notification preference information from an update command.
    /// </summary>
    /// <param name="command">The UpdateNotificationPreferenceCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateNotificationPreferenceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        Maintenance = command.Maintenance;
        Speed = command.Speed;
        Incidents = command.Incidents;
    }
}
