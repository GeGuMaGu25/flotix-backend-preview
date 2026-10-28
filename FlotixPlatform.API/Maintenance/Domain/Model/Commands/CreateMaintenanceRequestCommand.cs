namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to create a maintenance request
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="Description">The description of the fault to repair.</param>
public record CreateMaintenanceRequestCommand(Guid VehicleId, Guid MechanicId, string Description);
