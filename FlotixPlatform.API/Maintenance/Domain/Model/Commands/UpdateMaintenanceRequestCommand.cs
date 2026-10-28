using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to update a maintenance request
/// </summary>
/// <param name="MaintenanceRequestId">The identifier of the maintenance request to update</param>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="Description">The description of the fault to repair.</param>
/// <param name="EstimatedCost">The estimated cost quoted by the workshop.</param>
/// <param name="Status">The current status of the maintenance request.</param>
public record UpdateMaintenanceRequestCommand(Guid MaintenanceRequestId, Guid MechanicId, string Description, decimal EstimatedCost, MaintenanceRequestStatus Status);
