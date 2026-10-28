namespace FlotixPlatform.API.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to update a notification preference
/// </summary>
/// <param name="NotificationPreferenceId">The identifier of the notification preference to update</param>
/// <param name="Maintenance">Whether maintenance notifications are enabled.</param>
/// <param name="Speed">Whether speeding notifications are enabled.</param>
/// <param name="Incidents">Whether incident notifications are enabled.</param>
public record UpdateNotificationPreferenceCommand(int NotificationPreferenceId, bool Maintenance, bool Speed, bool Incidents);
