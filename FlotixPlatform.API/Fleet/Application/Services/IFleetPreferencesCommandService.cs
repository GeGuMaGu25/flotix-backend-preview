using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Domain.Model.Commands;
using FlotixPlatform.API.Fleet.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Fleet.Application.Services;

/// <summary>
///     Command service interface for fleet preferences operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change the fleet preferences.
/// </remarks>
public interface IFleetPreferencesCommandService
{
    /// <summary>
    ///     Handle the update fleet preferences command.
    /// </summary>
    /// <param name="command">UpdateFleetPreferencesCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated FleetPreferences on success, or the error reason on failure.
    /// </returns>
    Task<Result<FleetPreferences, UpdateFleetPreferencesError>> Handle(UpdateFleetPreferencesCommand command, CancellationToken cancellationToken = default);
}
