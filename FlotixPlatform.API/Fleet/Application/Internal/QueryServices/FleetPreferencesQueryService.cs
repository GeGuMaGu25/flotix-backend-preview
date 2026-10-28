using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Application.Services;
using FlotixPlatform.API.Fleet.Domain.Model.Queries;
using FlotixPlatform.API.Fleet.Domain.Repositories;

namespace FlotixPlatform.API.Fleet.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying the fleet preferences.
/// </summary>
/// <param name="fleetPreferencesRepository">Repository for accessing fleet preferences data.</param>
public class FleetPreferencesQueryService(IFleetPreferencesRepository fleetPreferencesRepository)
    : IFleetPreferencesQueryService
{
    /// <inheritdoc />
    public async Task<FleetPreferences?> Handle(GetFleetPreferencesQuery query,
        CancellationToken cancellationToken = default) =>
        await fleetPreferencesRepository.FindByIdAsync(FleetPreferences.DefaultId, cancellationToken);
}
