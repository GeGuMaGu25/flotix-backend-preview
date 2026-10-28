using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an IoT device.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="SerialNumber">The serial number of the IoT device. (required)</param>
/// <param name="ConnectionStatus">The connection status of the IoT device.</param>
public record CreateIotDeviceResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int VehicleId,
    [Required(ErrorMessage = "RequiredSerialNumber")]
    [StringLength(40, ErrorMessage = "StringLengthField")]
    string SerialNumber,
    ConnectionStatus ConnectionStatus);
