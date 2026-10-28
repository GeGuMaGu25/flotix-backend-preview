namespace FlotixPlatform.API.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a telemetry record.
/// </summary>
public enum CreateTelemetryDataError
{
	/// <summary>
	/// The provided telemetry record data is invalid.
	/// </summary>
	InvalidTelemetryData,

	/// <summary>
	/// A resource referenced by the command does not exist.
	/// </summary>
	ReferencedResourceNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
