using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Commerce.Application.Services;

/// <summary>
///     Command service interface for IoT catalog item operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change IoT catalog items.
/// </remarks>
public interface IIotCatalogItemCommandService
{
    /// <summary>
    ///     Handle the create IoT catalog item command.
    /// </summary>
    /// <param name="command">CreateIotCatalogItemCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created IotCatalogItem on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotCatalogItem, CreateIotCatalogItemError>> Handle(CreateIotCatalogItemCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update IoT catalog item command.
    /// </summary>
    /// <param name="command">UpdateIotCatalogItemCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated IotCatalogItem on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotCatalogItem, UpdateIotCatalogItemError>> Handle(UpdateIotCatalogItemCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete IoT catalog item command.
    /// </summary>
    /// <param name="command">DeleteIotCatalogItemCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted IotCatalogItem on success, or the error reason on failure.
    /// </returns>
    Task<Result<IotCatalogItem, DeleteIotCatalogItemError>> Handle(DeleteIotCatalogItemCommand command, CancellationToken cancellationToken = default);
}
