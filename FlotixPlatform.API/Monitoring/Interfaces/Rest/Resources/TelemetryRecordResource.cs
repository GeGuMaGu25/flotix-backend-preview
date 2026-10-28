namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a telemetry record.
/// </summary>
/// <param name="Id">The server-generated ID of the telemetry record</param>
/// <param name="DeviceId">The identifier of the reporting IoT device.</param>
/// <param name="Latitude">The latitude of the reported position.</param>
/// <param name="Longitude">The longitude of the reported position.</param>
/// <param name="Speed">The reported speed in km/h.</param>
/// <param name="Timestamp">The moment the reading was taken by the device.</param>
public record TelemetryRecordResource(
    int Id,
    int DeviceId,
    double Latitude,
    double Longitude,
    int Speed,
    DateTimeOffset Timestamp);
