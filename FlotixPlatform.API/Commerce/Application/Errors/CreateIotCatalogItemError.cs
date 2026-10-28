namespace FlotixPlatform.API.Commerce.Application.Errors;

/// <summary>
/// Represents errors that can occur when creating an IoT catalog item.
/// </summary>
public enum CreateIotCatalogItemError
{
	/// <summary>
	/// The provided IoT catalog item data is invalid.
	/// </summary>
	InvalidIotCatalogItem,

	/// <summary>
	/// An IoT catalog item with the same SKU already exists.
	/// </summary>
	DuplicateIotCatalogItemSku,

	/// <summary>
	/// An unexpected error occurred during the operation.
	/// </summary>
	UnexpectedError
}
