using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.Fleet.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a vehicle.
/// </summary>
/// <param name="OwnerId">The identifier of the owner user.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Plate">The license plate of the vehicle. (required)</param>
/// <param name="VehicleType">The type of the vehicle (e.g. Camión de carga). (required)</param>
/// <param name="CurrentMileage">The current mileage of the vehicle in kilometers.</param>
/// <param name="Status">The current status of the vehicle.</param>
public record CreateVehicleResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int OwnerId,
    int? DriverId,
    [Required(ErrorMessage = "RequiredPlate")]
    [StringLength(12, ErrorMessage = "StringLengthField")]
    string Plate,
    [Required(ErrorMessage = "RequiredVehicleType")]
    [StringLength(60, ErrorMessage = "StringLengthField")]
    string VehicleType,
    [Range(0, int.MaxValue, ErrorMessage = "RangeField")]
    int CurrentMileage,
    VehicleStatus Status);
