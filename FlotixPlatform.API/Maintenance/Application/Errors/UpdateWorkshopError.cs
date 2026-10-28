namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating a workshop.
/// </summary>
public enum UpdateWorkshopError
{
	/// <summary>
	/// The workshop to update does not exist.
	/// </summary>
	WorkshopNotFound,

	/// <summary>
	/// The provided workshop data is invalid.
	/// </summary>
	InvalidWorkshop,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
