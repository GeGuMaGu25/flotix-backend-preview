namespace FlotixPlatform.API.Notifications.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an alert.
/// </summary>
public enum CreateAlertError
{
	/// <summary>
	/// The provided alert data is invalid.
	/// </summary>
	InvalidAlert,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
