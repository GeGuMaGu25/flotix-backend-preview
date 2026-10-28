namespace FlotixPlatform.API.Fuel.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a fuel record.
/// </summary>
public enum CreateFuelRecordError
{
	/// <summary>
	/// The provided fuel record data is invalid.
	/// </summary>
	InvalidFuelRecord,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
