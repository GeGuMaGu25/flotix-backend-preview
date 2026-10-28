using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Application.Errors;
using FlotixPlatform.API.Fleet.Application.Services;
using FlotixPlatform.API.Fleet.Domain.Model.Commands;
using FlotixPlatform.API.Fleet.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Fleet.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling fleet preferences commands.
/// </summary>
/// <param name="fleetPreferencesRepository">Repository for accessing fleet preferences data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class FleetPreferencesCommandService(
    IFleetPreferencesRepository fleetPreferencesRepository,
    IUnitOfWork unitOfWork,
    ILogger<FleetPreferencesCommandService> logger)
    : IFleetPreferencesCommandService
{
    /// <inheritdoc />
    public async Task<Result<FleetPreferences, UpdateFleetPreferencesError>> Handle(UpdateFleetPreferencesCommand command,
        CancellationToken cancellationToken = default)
    {
        var fleetPreferences = await fleetPreferencesRepository.FindByIdAsync(FleetPreferences.DefaultId, cancellationToken);
        if (fleetPreferences is null)
            return new Result<FleetPreferences, UpdateFleetPreferencesError>.Failure(
                UpdateFleetPreferencesError.FleetPreferencesNotFound);

        try
        {
            fleetPreferences.UpdateInformation(command);
            fleetPreferencesRepository.Update(fleetPreferences);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<FleetPreferences, UpdateFleetPreferencesError>.Success(fleetPreferences);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating fleet preferences");
            return new Result<FleetPreferences, UpdateFleetPreferencesError>.Failure(
                UpdateFleetPreferencesError.InvalidFleetPreferences);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating fleet preferences");
            return new Result<FleetPreferences, UpdateFleetPreferencesError>.Failure(
                UpdateFleetPreferencesError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating fleet preferences");
            return new Result<FleetPreferences, UpdateFleetPreferencesError>.Failure(
                UpdateFleetPreferencesError.UnexpectedError);
        }
    }
}
