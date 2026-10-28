namespace FlotixPlatform.API.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to delete a notification preference
/// </summary>
/// <param name="NotificationPreferenceId">The identifier of the notification preference to delete</param>
public record DeleteNotificationPreferenceCommand(int NotificationPreferenceId);
