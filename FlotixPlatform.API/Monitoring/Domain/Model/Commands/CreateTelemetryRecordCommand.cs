namespace FlotixPlatform.API.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to create a telemetry record
/// </summary>
/// <param name="DeviceId">The identifier of the reporting IoT device.</param>
/// <param name="Latitude">The latitude of the reported position.</param>
/// <param name="Longitude">The longitude of the reported position.</param>
/// <param name="Speed">The reported speed in km/h.</param>
/// <param name="Timestamp">The moment the reading was taken by the device.</param>
public record CreateTelemetryRecordCommand(int DeviceId, double Latitude, double Longitude, int Speed, DateTimeOffset Timestamp);
