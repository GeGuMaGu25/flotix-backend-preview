namespace FlotixPlatform.API.Commerce.Application.Errors;

/// <summary>
/// Represents errors that can occur when updating an IoT catalog item.
/// </summary>
public enum UpdateIotCatalogItemError
{
	/// <summary>
	/// The IoT catalog item to update does not exist.
	/// </summary>
	IotCatalogItemNotFound,

	/// <summary>
	/// The provided IoT catalog item data is invalid.
	/// </summary>
	InvalidIotCatalogItem,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
