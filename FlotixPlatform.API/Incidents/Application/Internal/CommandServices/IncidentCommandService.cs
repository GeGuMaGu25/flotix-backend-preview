using FlotixPlatform.API.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.API.Incidents.Application.Errors;
using FlotixPlatform.API.Incidents.Application.Services;
using FlotixPlatform.API.Incidents.Domain.Model.Commands;
using FlotixPlatform.API.Incidents.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Incidents.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling incident commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for incidents.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="incidentRepository">Repository for accessing incident data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IIncidentRepository">IIncidentRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class IncidentCommandService(
    IIncidentRepository incidentRepository,
    IUnitOfWork unitOfWork,
    ILogger<IncidentCommandService> logger)
    : IIncidentCommandService
{
    /// <inheritdoc />
    public async Task<Result<Incident, CreateIncidentError>> Handle(CreateIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var incident = new Incident(command);
            await incidentRepository.AddAsync(incident, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Incident, CreateIncidentError>.Success(incident);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating incident");
            return new Result<Incident, CreateIncidentError>.Failure(CreateIncidentError.InvalidIncident);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating incident");
            return new Result<Incident, CreateIncidentError>.Failure(CreateIncidentError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating incident");
            return new Result<Incident, CreateIncidentError>.Failure(CreateIncidentError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Incident, UpdateIncidentError>> Handle(UpdateIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null)
            return new Result<Incident, UpdateIncidentError>.Failure(
                UpdateIncidentError.IncidentNotFound);

        try
        {
            incident.UpdateInformation(command);
            incidentRepository.Update(incident);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Incident, UpdateIncidentError>.Success(incident);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating incident");
            return new Result<Incident, UpdateIncidentError>.Failure(UpdateIncidentError.InvalidIncident);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating incident");
            return new Result<Incident, UpdateIncidentError>.Failure(UpdateIncidentError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating incident");
            return new Result<Incident, UpdateIncidentError>.Failure(UpdateIncidentError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Incident, DeleteIncidentError>> Handle(DeleteIncidentCommand command,
        CancellationToken cancellationToken = default)
    {
        var incident = await incidentRepository.FindByIdAsync(command.IncidentId, cancellationToken);
        if (incident is null)
            return new Result<Incident, DeleteIncidentError>.Failure(
                DeleteIncidentError.IncidentNotFound);

        try
        {
            incidentRepository.Remove(incident);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Incident, DeleteIncidentError>.Success(incident);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting incident");
            return new Result<Incident, DeleteIncidentError>.Failure(DeleteIncidentError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting incident");
            return new Result<Incident, DeleteIncidentError>.Failure(DeleteIncidentError.UnexpectedError);
        }
    }
}
