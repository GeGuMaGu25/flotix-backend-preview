namespace FlotixPlatform.API.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a notification preference.
/// </summary>
public enum CreateNotificationPreferenceError
{
	/// <summary>
	/// The provided notification preference data is invalid.
	/// </summary>
	InvalidNotificationPreference,

	/// <summary>
	/// A notification preference with the same user ID already exists.
	/// </summary>
	DuplicateNotificationPreferenceUserId,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
