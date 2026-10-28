namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a workshop.
/// </summary>
public enum CreateWorkshopError
{
	/// <summary>
	/// The provided workshop data is invalid.
	/// </summary>
	InvalidWorkshop,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
