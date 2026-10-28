namespace FlotixPlatform.API.Fleet.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating the fleet preferences.
/// </summary>
public enum UpdateFleetPreferencesError
{
	/// <summary>
	/// The fleet preferences have not been seeded.
	/// </summary>
	FleetPreferencesNotFound,

	/// <summary>
	/// The provided fleet preferences data is invalid.
	/// </summary>
	InvalidFleetPreferences,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
