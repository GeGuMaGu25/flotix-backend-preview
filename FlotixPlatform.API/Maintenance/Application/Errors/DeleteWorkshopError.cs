namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a workshop.
/// </summary>
public enum DeleteWorkshopError
{
	/// <summary>
	/// The workshop to delete does not exist.
	/// </summary>
	WorkshopNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
