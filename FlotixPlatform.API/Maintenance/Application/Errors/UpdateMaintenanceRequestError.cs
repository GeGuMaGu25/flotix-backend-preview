namespace FlotixPlatform.API.Maintenance.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating a maintenance request.
/// </summary>
public enum UpdateMaintenanceRequestError
{
	/// <summary>
	/// The maintenance request to update does not exist.
	/// </summary>
	MaintenanceRequestNotFound,

	/// <summary>
	/// The provided maintenance request data is invalid.
	/// </summary>
	InvalidMaintenanceRequest,

	/// <summary>
	/// A completed or rejected maintenance request cannot change its status.
	/// </summary>
	MaintenanceRequestAlreadyClosed,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
