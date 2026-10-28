namespace FlotixPlatform.API.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a notification preference.
/// </summary>
public enum DeleteNotificationPreferenceError
{
	/// <summary>
	/// The notification preference to delete does not exist.
	/// </summary>
	NotificationPreferenceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
