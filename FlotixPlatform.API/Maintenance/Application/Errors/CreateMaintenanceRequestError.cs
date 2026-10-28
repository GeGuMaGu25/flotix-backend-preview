namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a maintenance request.
/// </summary>
public enum CreateMaintenanceRequestError
{
	/// <summary>
	/// The provided maintenance request data is invalid.
	/// </summary>
	InvalidMaintenanceRequest,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
