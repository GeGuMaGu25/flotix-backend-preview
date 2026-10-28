namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the latest location of a vehicle, published in real time by the IoT Telemetry Processor.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle</param>
/// <param name="DeviceId">The identifier of the reporting IoT device</param>
/// <param name="Latitude">The latitude of the vehicle</param>
/// <param name="Longitude">The longitude of the vehicle</param>
/// <param name="Speed">The speed of the vehicle in km/h</param>
/// <param name="RecordedAt">The moment the reading was taken by the device</param>
public record VehicleLocationResource(
    Guid VehicleId,
    Guid DeviceId,
    decimal Latitude,
    decimal Longitude,
    decimal Speed,
    DateTimeOffset RecordedAt);
