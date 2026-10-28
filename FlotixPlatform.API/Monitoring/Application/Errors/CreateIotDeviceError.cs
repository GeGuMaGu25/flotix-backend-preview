namespace FlotixPlatform.API.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an IoT device.
/// </summary>
public enum CreateIotDeviceError
{
	/// <summary>
	/// The provided IoT device data is invalid.
	/// </summary>
	InvalidIotDevice,

	/// <summary>
	/// An IoT device with the same serial number already exists.
	/// </summary>
	DuplicateIotDeviceSerialNumber,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
