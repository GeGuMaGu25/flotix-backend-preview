namespace FlotixPlatform.API.Fleet.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting a vehicle.
/// </summary>
public enum DeleteVehicleError
{
	/// <summary>
	/// The vehicle to delete does not exist.
	/// </summary>
	VehicleNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
