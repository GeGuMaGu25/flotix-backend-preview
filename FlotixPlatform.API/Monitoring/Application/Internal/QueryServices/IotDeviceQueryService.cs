using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Model.Queries;
using FlotixPlatform.API.Monitoring.Domain.Repositories;

namespace FlotixPlatform.API.Monitoring.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying IoT devices.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving IoT devices.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="iotDeviceRepository">Repository for accessing IoT device data.</param>
public class IotDeviceQueryService(IIotDeviceRepository iotDeviceRepository)
    : IIotDeviceQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<IotDevice>> Handle(GetAllIotDevicesQuery query,
        CancellationToken cancellationToken = default) =>
        await iotDeviceRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IotDevice?> Handle(GetIotDeviceByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await iotDeviceRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<IotDevice>> Handle(GetAllIotDevicesByVehicleIdQuery query,
        CancellationToken cancellationToken = default) =>
        await iotDeviceRepository.FindByVehicleIdAsync(query.VehicleId, cancellationToken);
}
