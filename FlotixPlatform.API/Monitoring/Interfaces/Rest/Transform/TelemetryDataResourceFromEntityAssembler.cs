using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a TelemetryDataResource from a TelemetryData.
/// </summary>
public static class TelemetryDataResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a TelemetryDataResource from a TelemetryData.
    /// </summary>
    /// <param name="entity">The TelemetryData entity</param>
    /// <returns>
    ///     A TelemetryDataResource assembled from the TelemetryData
    /// </returns>
    public static TelemetryDataResource ToResourceFromEntity(TelemetryData entity) =>
        new(entity.Id, entity.DeviceId, entity.Latitude, entity.Longitude, entity.Speed, entity.RecordedAt);
}
