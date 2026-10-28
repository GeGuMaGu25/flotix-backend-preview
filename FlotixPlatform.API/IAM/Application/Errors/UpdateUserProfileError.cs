namespace FlotixPlatform.API.IAM.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating the profile of a user.
/// </summary>
public enum UpdateUserProfileError
{
	/// <summary>
	/// The user to update does not exist.
	/// </summary>
	UserNotFound,

	/// <summary>
	/// The provided user data is invalid.
	/// </summary>
	InvalidUser,

	/// <summary>
	/// A driver profile was left without a license number or its expiration date.
	/// </summary>
	DriverLicenseRequired,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
