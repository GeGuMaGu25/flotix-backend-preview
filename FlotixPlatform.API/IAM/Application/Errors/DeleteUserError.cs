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
	/// The user is referenced by vehicles, records or orders and cannot be deleted.
	/// </summary>
	UserHasDependents,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
