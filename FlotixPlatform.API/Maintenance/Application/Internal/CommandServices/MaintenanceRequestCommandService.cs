using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Application.Services;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Maintenance.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling maintenance request commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for maintenance requests.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="maintenanceRequestRepository">Repository for accessing maintenance request data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IMaintenanceRequestRepository">IMaintenanceRequestRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class MaintenanceRequestCommandService(
    IMaintenanceRequestRepository maintenanceRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<MaintenanceRequestCommandService> logger)
    : IMaintenanceRequestCommandService
{
    /// <inheritdoc />
    public async Task<Result<MaintenanceRequest, CreateMaintenanceRequestError>> Handle(CreateMaintenanceRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var maintenanceRequest = new MaintenanceRequest(command);
            await maintenanceRequestRepository.AddAsync(maintenanceRequest, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaintenanceRequest, CreateMaintenanceRequestError>.Success(maintenanceRequest);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating maintenance request");
            return new Result<MaintenanceRequest, CreateMaintenanceRequestError>.Failure(CreateMaintenanceRequestError.InvalidMaintenanceRequest);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating maintenance request");
            return new Result<MaintenanceRequest, CreateMaintenanceRequestError>.Failure(CreateMaintenanceRequestError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating maintenance request");
            return new Result<MaintenanceRequest, CreateMaintenanceRequestError>.Failure(CreateMaintenanceRequestError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<MaintenanceRequest, UpdateMaintenanceRequestError>> Handle(UpdateMaintenanceRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var maintenanceRequest = await maintenanceRequestRepository.FindByIdAsync(command.MaintenanceRequestId, cancellationToken);
        if (maintenanceRequest is null)
            return new Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Failure(
                UpdateMaintenanceRequestError.MaintenanceRequestNotFound);

        try
        {
            maintenanceRequest.UpdateInformation(command);
            maintenanceRequestRepository.Update(maintenanceRequest);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Success(maintenanceRequest);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Business rule rejected updating maintenance request");
            return new Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Failure(UpdateMaintenanceRequestError.MaintenanceRequestAlreadyClosed);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating maintenance request");
            return new Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Failure(UpdateMaintenanceRequestError.InvalidMaintenanceRequest);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating maintenance request");
            return new Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Failure(UpdateMaintenanceRequestError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating maintenance request");
            return new Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Failure(UpdateMaintenanceRequestError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<MaintenanceRequest, DeleteMaintenanceRequestError>> Handle(DeleteMaintenanceRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        var maintenanceRequest = await maintenanceRequestRepository.FindByIdAsync(command.MaintenanceRequestId, cancellationToken);
        if (maintenanceRequest is null)
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Failure(
                DeleteMaintenanceRequestError.MaintenanceRequestNotFound);

        try
        {
            maintenanceRequestRepository.Remove(maintenanceRequest);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Success(maintenanceRequest);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting maintenance request");
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Failure(DeleteMaintenanceRequestError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting maintenance request");
            return new Result<MaintenanceRequest, DeleteMaintenanceRequestError>.Failure(DeleteMaintenanceRequestError.UnexpectedError);
        }
    }
}
