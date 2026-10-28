using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an IoT device.
/// </summary>
/// <param name="Id">The server-generated ID of the IoT device</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="SerialNumber">The serial number of the IoT device.</param>
/// <param name="ConnectionStatus">The connection status of the IoT device.</param>
public record IotDeviceResource(
    int Id,
    int VehicleId,
    string SerialNumber,
    ConnectionStatus ConnectionStatus);
