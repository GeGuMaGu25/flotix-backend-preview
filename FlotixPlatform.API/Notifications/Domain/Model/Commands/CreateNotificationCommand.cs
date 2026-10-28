namespace FlotixPlatform.API.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to create a notification
/// </summary>
/// <param name="UserId">The identifier of the recipient user.</param>
/// <param name="AlertId">The identifier of the related alert, if any.</param>
/// <param name="Message">The message of the notification.</param>
public record CreateNotificationCommand(int UserId, int? AlertId, string Message);
