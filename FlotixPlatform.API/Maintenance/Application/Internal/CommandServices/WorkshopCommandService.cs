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
///     Application service for handling workshop commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for workshops.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="workshopRepository">Repository for accessing workshop data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IWorkshopRepository">IWorkshopRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class WorkshopCommandService(
    IWorkshopRepository workshopRepository,
    IUnitOfWork unitOfWork,
    ILogger<WorkshopCommandService> logger)
    : IWorkshopCommandService
{
    /// <inheritdoc />
    public async Task<Result<Workshop, CreateWorkshopError>> Handle(CreateWorkshopCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var workshop = new Workshop(command);
            await workshopRepository.AddAsync(workshop, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Workshop, CreateWorkshopError>.Success(workshop);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating workshop");
            return new Result<Workshop, CreateWorkshopError>.Failure(CreateWorkshopError.InvalidWorkshop);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating workshop");
            return new Result<Workshop, CreateWorkshopError>.Failure(CreateWorkshopError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating workshop");
            return new Result<Workshop, CreateWorkshopError>.Failure(CreateWorkshopError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Workshop, UpdateWorkshopError>> Handle(UpdateWorkshopCommand command,
        CancellationToken cancellationToken = default)
    {
        var workshop = await workshopRepository.FindByIdAsync(command.WorkshopId, cancellationToken);
        if (workshop is null)
            return new Result<Workshop, UpdateWorkshopError>.Failure(
                UpdateWorkshopError.WorkshopNotFound);

        try
        {
            workshop.UpdateInformation(command);
            workshopRepository.Update(workshop);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Workshop, UpdateWorkshopError>.Success(workshop);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating workshop");
            return new Result<Workshop, UpdateWorkshopError>.Failure(UpdateWorkshopError.InvalidWorkshop);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating workshop");
            return new Result<Workshop, UpdateWorkshopError>.Failure(UpdateWorkshopError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating workshop");
            return new Result<Workshop, UpdateWorkshopError>.Failure(UpdateWorkshopError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Workshop, DeleteWorkshopError>> Handle(DeleteWorkshopCommand command,
        CancellationToken cancellationToken = default)
    {
        var workshop = await workshopRepository.FindByIdAsync(command.WorkshopId, cancellationToken);
        if (workshop is null)
            return new Result<Workshop, DeleteWorkshopError>.Failure(
                DeleteWorkshopError.WorkshopNotFound);

        try
        {
            workshopRepository.Remove(workshop);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Workshop, DeleteWorkshopError>.Success(workshop);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting workshop");
            return new Result<Workshop, DeleteWorkshopError>.Failure(DeleteWorkshopError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting workshop");
            return new Result<Workshop, DeleteWorkshopError>.Failure(DeleteWorkshopError.UnexpectedError);
        }
    }
}
