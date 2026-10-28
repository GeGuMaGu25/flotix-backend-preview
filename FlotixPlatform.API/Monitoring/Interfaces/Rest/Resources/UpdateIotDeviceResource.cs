using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update an IoT device.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="ConnectionStatus">The connection status of the IoT device.</param>
public record UpdateIotDeviceResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int VehicleId,
    ConnectionStatus ConnectionStatus);
