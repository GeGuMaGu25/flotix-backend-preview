namespace FlotixPlatform.API.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to create a notification preference
/// </summary>
/// <param name="UserId">The identifier of the user.</param>
/// <param name="Maintenance">Whether maintenance notifications are enabled.</param>
/// <param name="Speed">Whether speeding notifications are enabled.</param>
/// <param name="Incidents">Whether incident notifications are enabled.</param>
public record CreateNotificationPreferenceCommand(int UserId, bool Maintenance, bool Speed, bool Incidents);
