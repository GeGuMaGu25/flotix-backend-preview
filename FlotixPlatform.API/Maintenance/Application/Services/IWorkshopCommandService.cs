using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Command service interface for workshop operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change workshops.
/// </remarks>
public interface IWorkshopCommandService
{
    /// <summary>
    ///     Handle the create workshop command.
    /// </summary>
    /// <param name="command">CreateWorkshopCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created Workshop on success, or the error reason on failure.
    /// </returns>
    Task<Result<Workshop, CreateWorkshopError>> Handle(CreateWorkshopCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update workshop command.
    /// </summary>
    /// <param name="command">UpdateWorkshopCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated Workshop on success, or the error reason on failure.
    /// </returns>
    Task<Result<Workshop, UpdateWorkshopError>> Handle(UpdateWorkshopCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete workshop command.
    /// </summary>
    /// <param name="command">DeleteWorkshopCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted Workshop on success, or the error reason on failure.
    /// </returns>
    Task<Result<Workshop, DeleteWorkshopError>> Handle(DeleteWorkshopCommand command, CancellationToken cancellationToken = default);
}
