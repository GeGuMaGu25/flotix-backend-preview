using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Monitoring.Domain;

public sealed class IotDevice : Entity
{
    private IotDevice() { }

    public IotDevice(string vehicleId, string serialNumber, ConnectionStatus connectionStatus)
    {
        SerialNumber = serialNumber.Trim().ToUpperInvariant();
        Update(vehicleId, connectionStatus);
    }

    public string VehicleId { get; private set; } = null!;

    public string SerialNumber { get; private set; } = null!;

    public ConnectionStatus ConnectionStatus { get; private set; }

    public void Update(string vehicleId, ConnectionStatus connectionStatus)
    {
        VehicleId = vehicleId;
        ConnectionStatus = connectionStatus;
    }
}
