using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a TelemetryRecordResource from a TelemetryRecord.
/// </summary>
public static class TelemetryRecordResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a TelemetryRecordResource from a TelemetryRecord.
    /// </summary>
    /// <param name="entity">The TelemetryRecord entity</param>
    /// <returns>
    ///     A TelemetryRecordResource assembled from the TelemetryRecord
    /// </returns>
    public static TelemetryRecordResource ToResourceFromEntity(TelemetryRecord entity) =>
        new(entity.Id, entity.DeviceId, entity.Latitude, entity.Longitude, entity.Speed, entity.Timestamp);
}
