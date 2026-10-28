namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an IoT device.
/// </summary>
/// <param name="Id">The server-generated ID of the IoT device</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="SerialNumber">The serial number of the IoT device.</param>
/// <param name="IsConnected">Whether the IoT device is connected.</param>
public record IotDeviceResource(
    Guid Id,
    Guid VehicleId,
    string SerialNumber,
    bool IsConnected);
