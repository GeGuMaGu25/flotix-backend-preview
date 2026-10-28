namespace FlotixPlatform.API.Fleet.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a vehicle.
/// </summary>
public enum CreateVehicleError
{
	/// <summary>
	/// The provided vehicle data is invalid.
	/// </summary>
	InvalidVehicle,

	/// <summary>
	/// A vehicle with the same plate already exists.
	/// </summary>
	DuplicateVehiclePlate,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
