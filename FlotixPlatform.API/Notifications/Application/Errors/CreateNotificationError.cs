namespace FlotixPlatform.API.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a notification.
/// </summary>
public enum CreateNotificationError
{
	/// <summary>
	/// The provided notification data is invalid.
	/// </summary>
	InvalidNotification,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
