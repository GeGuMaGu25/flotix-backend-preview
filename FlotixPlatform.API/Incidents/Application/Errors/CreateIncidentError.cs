namespace FlotixPlatform.API.Incidents.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an incident.
/// </summary>
public enum CreateIncidentError
{
	/// <summary>
	/// The provided incident data is invalid.
	/// </summary>
	InvalidIncident,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
