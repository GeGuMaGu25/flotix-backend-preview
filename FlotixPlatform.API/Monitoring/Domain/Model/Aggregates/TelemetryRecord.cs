using FlotixPlatform.API.Monitoring.Domain.Model.Commands;

namespace FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;

/// <summary>
///     Represents a position and speed reading reported by an IoT device; readings are immutable.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateTelemetryRecordCommand and is immutable afterwards.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class TelemetryRecord
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected TelemetryRecord()
    {
    }

    /// <summary>
    ///     Creates a new TelemetryRecord from a creation command.
    /// </summary>
    /// <param name="command">The CreateTelemetryRecordCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public TelemetryRecord(CreateTelemetryRecordCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.DeviceId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.Latitude, -90);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Latitude, 90);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.Longitude, -180);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Longitude, 180);
        ArgumentOutOfRangeException.ThrowIfNegative(command.Speed);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Speed, 400);

        DeviceId = command.DeviceId;
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Speed = command.Speed;
        Timestamp = command.Timestamp;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this telemetry record.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the reporting IoT device.
    /// </summary>
    public int DeviceId { get; private set; }

    /// <summary>
    ///     Gets the latitude of the reported position.
    /// </summary>
    public double Latitude { get; private set; }

    /// <summary>
    ///     Gets the longitude of the reported position.
    /// </summary>
    public double Longitude { get; private set; }

    /// <summary>
    ///     Gets the reported speed in km/h.
    /// </summary>
    public int Speed { get; private set; }

    /// <summary>
    ///     Gets the moment the reading was taken by the device.
    /// </summary>
    public DateTimeOffset Timestamp { get; private set; }
}
