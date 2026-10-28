using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Maintenance.Application.Services;

/// <summary>
///     Command service interface for maintenance request operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change maintenance requests.
/// </remarks>
public interface IMaintenanceRequestCommandService
{
    /// <summary>
    ///     Handle the create maintenance request command.
    /// </summary>
    /// <param name="command">CreateMaintenanceRequestCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, CreateMaintenanceRequestError>> Handle(CreateMaintenanceRequestCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update maintenance request command.
    /// </summary>
    /// <param name="command">UpdateMaintenanceRequestCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, UpdateMaintenanceRequestError>> Handle(UpdateMaintenanceRequestCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete maintenance request command.
    /// </summary>
    /// <param name="command">DeleteMaintenanceRequestCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted MaintenanceRequest on success, or the error reason on failure.
    /// </returns>
    Task<Result<MaintenanceRequest, DeleteMaintenanceRequestError>> Handle(DeleteMaintenanceRequestCommand command, CancellationToken cancellationToken = default);
}
