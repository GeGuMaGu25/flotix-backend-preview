using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Maintenance.Domain.Repositories;

/// <summary>
///     The maintenance request repository contract.
/// </summary>
public interface IMaintenanceRequestRepository : IBaseRepository<MaintenanceRequest>
{
    /// <summary>
    ///     Find maintenance requests by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the maintenance request objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<MaintenanceRequest>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find maintenance requests by mechanic ID
    /// </summary>
    /// <param name="mechanicId">The mechanic ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the maintenance request objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<MaintenanceRequest>> FindByMechanicIdAsync(Guid mechanicId, CancellationToken cancellationToken = default);
}
