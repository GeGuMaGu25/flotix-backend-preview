using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a maintenance request.
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="FaultDescription">The description of the fault to repair. (required)</param>
/// <param name="EstimatedCost">The estimated cost of the repair.</param>
/// <param name="Status">The current status of the maintenance request.</param>
public record CreateMaintenanceRequestResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int VehicleId,
    int? MechanicId,
    [Required(ErrorMessage = "RequiredFaultDescription")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string FaultDescription,
    [Range(0, 1000000, ErrorMessage = "RangeField")]
    decimal EstimatedCost,
    MaintenanceRequestStatus Status);
