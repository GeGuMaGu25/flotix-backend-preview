using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to create a maintenance request
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="FaultDescription">The description of the fault to repair.</param>
/// <param name="EstimatedCost">The estimated cost of the repair.</param>
/// <param name="Status">The current status of the maintenance request.</param>
public record CreateMaintenanceRequestCommand(int VehicleId, int? MechanicId, string FaultDescription, decimal EstimatedCost, MaintenanceRequestStatus Status);
