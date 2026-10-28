using System.ComponentModel.DataAnnotations;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a maintenance request.
/// </summary>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="Description">The description of the fault to repair. (required)</param>
/// <param name="EstimatedCost">The estimated cost quoted by the workshop.</param>
/// <param name="Status">The current status of the maintenance request.</param>
public record UpdateMaintenanceRequestResource(
    Guid MechanicId,
    [Required(ErrorMessage = "RequiredDescription")]
    [StringLength(500, ErrorMessage = "StringLengthField")]
    string Description,
    [Range(0, 1000000, ErrorMessage = "RangeField")]
    decimal EstimatedCost,
    MaintenanceRequestStatus Status);
