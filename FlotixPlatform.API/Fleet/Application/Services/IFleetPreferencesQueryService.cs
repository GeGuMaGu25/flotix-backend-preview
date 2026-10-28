using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Domain.Model.Queries;

namespace FlotixPlatform.API.Fleet.Application.Services;

/// <summary>
///     Query service interface for fleet preferences operations.
/// </summary>
public interface IFleetPreferencesQueryService
{
	/// <summary>
	///     Handle the GetFleetPreferencesQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetFleetPreferencesQuery. It returns the single fleet preferences instance.
	/// </remarks>
	/// <param name="query">The GetFleetPreferencesQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The FleetPreferences object if seeded, or null otherwise</returns>
	Task<FleetPreferences?> Handle(GetFleetPreferencesQuery query, CancellationToken cancellationToken = default);
}
