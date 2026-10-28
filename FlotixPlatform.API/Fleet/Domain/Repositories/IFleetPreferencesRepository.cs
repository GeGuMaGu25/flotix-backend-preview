using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Fleet.Domain.Repositories;

/// <summary>
///     The fleet preferences repository contract.
/// </summary>
public interface IFleetPreferencesRepository : IBaseRepository<FleetPreferences>;
