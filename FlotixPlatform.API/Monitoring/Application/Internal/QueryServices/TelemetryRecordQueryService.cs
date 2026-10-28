using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Model.Queries;
using FlotixPlatform.API.Monitoring.Domain.Repositories;

namespace FlotixPlatform.API.Monitoring.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying telemetry records.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving telemetry records.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="telemetryRecordRepository">Repository for accessing telemetry record data.</param>
public class TelemetryRecordQueryService(ITelemetryRecordRepository telemetryRecordRepository)
    : ITelemetryRecordQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<TelemetryRecord>> Handle(GetAllTelemetryRecordsQuery query,
        CancellationToken cancellationToken = default) =>
        await telemetryRecordRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<TelemetryRecord?> Handle(GetTelemetryRecordByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await telemetryRecordRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<TelemetryRecord>> Handle(GetAllTelemetryRecordsByDeviceIdQuery query,
        CancellationToken cancellationToken = default) =>
        await telemetryRecordRepository.FindByDeviceIdAsync(query.DeviceId, cancellationToken);
}
