namespace FlotixPlatform.API.IAM.Application.Errors;

/// <summary>
/// Represents errors that can occur when signing in a user.
/// </summary>
public enum SignInError
{
	/// <summary>
	/// The email address or the password is incorrect.
	/// </summary>
	InvalidCredentials,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
