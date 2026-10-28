using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a telemetry record.
/// </summary>
/// <param name="DeviceId">The identifier of the reporting IoT device.</param>
/// <param name="Latitude">The latitude of the reported position.</param>
/// <param name="Longitude">The longitude of the reported position.</param>
/// <param name="Speed">The reported speed in km/h.</param>
/// <param name="RecordedAt">The moment the reading was taken by the device.</param>
public record CreateTelemetryDataResource(
    Guid DeviceId,
    [Range(-90, 90, ErrorMessage = "RangeField")]
    decimal Latitude,
    [Range(-180, 180, ErrorMessage = "RangeField")]
    decimal Longitude,
    [Range(0, 400, ErrorMessage = "RangeField")]
    decimal Speed,
    DateTimeOffset RecordedAt);
