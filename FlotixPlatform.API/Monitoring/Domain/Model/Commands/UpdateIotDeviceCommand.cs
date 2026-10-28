using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to update an IoT device
/// </summary>
/// <param name="IotDeviceId">The identifier of the IoT device to update</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="ConnectionStatus">The connection status of the IoT device.</param>
public record UpdateIotDeviceCommand(int IotDeviceId, int VehicleId, ConnectionStatus ConnectionStatus);
