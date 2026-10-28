namespace FlotixPlatform.API.IAM.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a user.
/// </summary>
public enum DeleteUserError
{
	/// <summary>
	/// The user to delete does not exist.
	/// </summary>
	UserNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
