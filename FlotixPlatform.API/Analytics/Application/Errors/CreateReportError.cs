namespace FlotixPlatform.API.Analytics.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating a report.
/// </summary>
public enum CreateReportError
{
	/// <summary>
	/// The provided report data is invalid.
	/// </summary>
	InvalidReport,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
