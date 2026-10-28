namespace FlotixPlatform.API.Commerce.Application.Errors;

/// <summary>
/// Represents errors that can occur when deleting an IoT catalog item.
/// </summary>
public enum DeleteIotCatalogItemError
{
	/// <summary>
	/// The IoT catalog item to delete does not exist.
	/// </summary>
	IotCatalogItemNotFound,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
