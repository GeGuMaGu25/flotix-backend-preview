namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update an IoT device.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="IsConnected">Whether the IoT device is connected.</param>
public record UpdateIotDeviceResource(
    Guid VehicleId,
    bool IsConnected);
