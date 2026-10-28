using FlotixPlatform.API.Fuel.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Fuel.Domain.Repositories;

/// <summary>
///     The fuel record repository contract.
/// </summary>
public interface IFuelRecordRepository : IBaseRepository<FuelRecord>
{
    /// <summary>
    ///     Find fuel records by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the fuel record objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<FuelRecord>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find fuel records by driver ID
    /// </summary>
    /// <param name="driverId">The driver ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the fuel record objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<FuelRecord>> FindByDriverIdAsync(int driverId, CancellationToken cancellationToken = default);
}
