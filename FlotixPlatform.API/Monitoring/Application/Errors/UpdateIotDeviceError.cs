namespace FlotixPlatform.API.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating an IoT device.
/// </summary>
public enum UpdateIotDeviceError
{
	/// <summary>
	/// The IoT device to update does not exist.
	/// </summary>
	IotDeviceNotFound,

	/// <summary>
	/// The provided IoT device data is invalid.
	/// </summary>
	InvalidIotDevice,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
