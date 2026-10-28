namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents a telemetry packet sent by an IoT device.
/// </summary>
/// <remarks>
///     Every field is required; the packet is validated by the domain so corrupt packets are discarded
///     and recorded in the error log.
/// </remarks>
/// <param name="DeviceId">The identifier of the reporting IoT device (required)</param>
/// <param name="Latitude">The latitude of the reported position, from -90 to 90 (required)</param>
/// <param name="Longitude">The longitude of the reported position, from -180 to 180 (required)</param>
/// <param name="Speed">The reported speed in km/h, from 0 to 400 (required)</param>
/// <param name="RecordedAt">The moment the reading was taken by the device (required)</param>
public record ReceiveTelemetryDataResource(
    Guid? DeviceId,
    decimal? Latitude,
    decimal? Longitude,
    decimal? Speed,
    DateTimeOffset? RecordedAt);
