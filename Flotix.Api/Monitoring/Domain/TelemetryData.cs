using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Monitoring.Domain;

/// <summary>A position/speed reading reported by an IoT device.</summary>
public sealed class TelemetryData : Entity
{
    private TelemetryData() { }

    public TelemetryData(string deviceId, double latitude, double longitude, int speed, DateTimeOffset timestamp)
    {
        DeviceId = deviceId;
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        Timestamp = timestamp;
    }

    public string DeviceId { get; private set; } = null!;

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public int Speed { get; private set; }

    public DateTimeOffset Timestamp { get; private set; }
}
