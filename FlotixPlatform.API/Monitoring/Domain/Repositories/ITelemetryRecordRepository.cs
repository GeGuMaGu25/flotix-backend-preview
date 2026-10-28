using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Monitoring.Domain.Repositories;

/// <summary>
///     The telemetry record repository contract.
/// </summary>
public interface ITelemetryRecordRepository : IBaseRepository<TelemetryRecord>
{
    /// <summary>
    ///     Find telemetry records by device ID
    /// </summary>
    /// <param name="deviceId">The device ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the telemetry record objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<TelemetryRecord>> FindByDeviceIdAsync(int deviceId, CancellationToken cancellationToken = default);
}
