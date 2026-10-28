namespace FlotixPlatform.API.Commerce.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an IoT order.
/// </summary>
public enum CreateIotOrderError
{
	/// <summary>
	/// The provided IoT order data is invalid.
	/// </summary>
	InvalidIotOrder,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
