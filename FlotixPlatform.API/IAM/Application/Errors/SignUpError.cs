namespace FlotixPlatform.API.IAM.Application.Errors;

/// <summary>
/// Represents errors that can occur when signing up a user.
/// </summary>
public enum SignUpError
{
	/// <summary>
	/// The provided user data is invalid.
	/// </summary>
	InvalidUser,

	/// <summary>
	/// A user with the same email address already exists.
	/// </summary>
	DuplicateUserEmail,

	/// <summary>
	/// A driver was signed up without a license number.
	/// </summary>
	DriverLicenseRequired,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
