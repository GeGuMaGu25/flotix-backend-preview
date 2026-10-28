using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Monitoring.Domain.Repositories;

/// <summary>
///     The IoT device repository contract.
/// </summary>
public interface IIotDeviceRepository : IBaseRepository<IotDevice>
{
    /// <summary>
    ///     Find IoT devices by vehicle ID
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the IoT device objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<IotDevice>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Check whether an IoT device with the given serial number exists
    /// </summary>
    /// <param name="serialNumber">The serial number to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     True if an IoT device with the serial number exists, false otherwise.
    /// </returns>
    Task<bool> ExistsBySerialNumberAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find an IoT device by serial number
    /// </summary>
    /// <param name="serialNumber">The serial number to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The IoT device object if found, or null otherwise.
    /// </returns>
    Task<IotDevice?> FindBySerialNumberAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default);
}
