namespace FlotixPlatform.API.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting an IoT device.
/// </summary>
public enum DeleteIotDeviceError
{
	/// <summary>
	/// The IoT device to delete does not exist.
	/// </summary>
	IotDeviceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
