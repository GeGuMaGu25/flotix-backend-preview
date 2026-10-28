namespace FlotixPlatform.API.Monitoring.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a telemetry record.
/// </summary>
public enum CreateTelemetryRecordError
{
	/// <summary>
	/// The provided telemetry record data is invalid.
	/// </summary>
	InvalidTelemetryRecord,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
