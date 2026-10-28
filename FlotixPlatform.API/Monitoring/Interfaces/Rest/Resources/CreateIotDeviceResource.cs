using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create an IoT device.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="SerialNumber">The serial number of the IoT device. (required)</param>
/// <param name="IsConnected">Whether the IoT device is connected.</param>
public record CreateIotDeviceResource(
    Guid VehicleId,
    [Required(ErrorMessage = "RequiredSerialNumber")]
    [StringLength(100, ErrorMessage = "StringLengthField")]
    string SerialNumber,
    bool IsConnected);
