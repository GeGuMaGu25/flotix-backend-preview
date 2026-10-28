using FlotixPlatform.API.Monitoring.Domain.Model.Commands;

namespace FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;

/// <summary>
///     Represents a position and speed reading reported by an IoT device; readings are immutable.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateTelemetryDataCommand and is immutable afterwards.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class TelemetryData
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected TelemetryData()
    {
    }

    /// <summary>
    ///     Creates a new TelemetryData from a creation command.
    /// </summary>
    /// <param name="command">The CreateTelemetryDataCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public TelemetryData(CreateTelemetryDataCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.DeviceId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.Latitude, -90m);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Latitude, 90m);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.Longitude, -180m);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Longitude, 180m);
        ArgumentOutOfRangeException.ThrowIfNegative(command.Speed);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Speed, 400m);

        DeviceId = command.DeviceId;
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Speed = command.Speed;
        RecordedAt = command.RecordedAt;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this telemetry record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the reporting IoT device.
    /// </summary>
    public Guid DeviceId { get; private set; }

    /// <summary>
    ///     Gets the latitude of the reported position.
    /// </summary>
    public decimal Latitude { get; private set; }

    /// <summary>
    ///     Gets the longitude of the reported position.
    /// </summary>
    public decimal Longitude { get; private set; }

    /// <summary>
    ///     Gets the reported speed in km/h.
    /// </summary>
    public decimal Speed { get; private set; }

    /// <summary>
    ///     Gets the moment the reading was taken by the device.
    /// </summary>
    public DateTimeOffset RecordedAt { get; private set; }
}
