using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a VehicleLocationResource from a TelemetryData.
/// </summary>
public static class VehicleLocationResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a VehicleLocationResource from a TelemetryData.
    /// </summary>
    /// <param name="vehicleId">The identifier of the vehicle equipped with the reporting device</param>
    /// <param name="entity">The TelemetryData entity</param>
    /// <returns>
    ///     A VehicleLocationResource assembled from the TelemetryData
    /// </returns>
    public static VehicleLocationResource ToResourceFromEntity(Guid vehicleId, TelemetryData entity) =>
        new(vehicleId, entity.DeviceId, entity.Latitude, entity.Longitude, entity.Speed, entity.RecordedAt);
}
