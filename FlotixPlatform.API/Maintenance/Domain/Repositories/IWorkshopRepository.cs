using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Maintenance.Domain.Repositories;

/// <summary>
///     The workshop repository contract.
/// </summary>
public interface IWorkshopRepository : IBaseRepository<Workshop>
{
    /// <summary>
    ///     Find workshops by mechanic ID
    /// </summary>
    /// <param name="mechanicId">The mechanic ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the workshop objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Workshop>> FindByMechanicIdAsync(int mechanicId, CancellationToken cancellationToken = default);
}
