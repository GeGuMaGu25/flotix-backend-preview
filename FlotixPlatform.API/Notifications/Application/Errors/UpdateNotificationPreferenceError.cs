namespace FlotixPlatform.API.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating a notification preference.
/// </summary>
public enum UpdateNotificationPreferenceError
{
	/// <summary>
	/// The notification preference to update does not exist.
	/// </summary>
	NotificationPreferenceNotFound,

	/// <summary>
	/// The provided notification preference data is invalid.
	/// </summary>
	InvalidNotificationPreference,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
