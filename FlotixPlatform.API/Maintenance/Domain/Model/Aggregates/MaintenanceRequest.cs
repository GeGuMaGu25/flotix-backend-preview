using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;

/// <summary>
///     Represents a request to repair a vehicle, optionally assigned to a mechanic.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateMaintenanceRequestCommand and updated through an UpdateMaintenanceRequestCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class MaintenanceRequest
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected MaintenanceRequest()
    {
        FaultDescription = string.Empty;
    }

    /// <summary>
    ///     Creates a new MaintenanceRequest from a creation command.
    /// </summary>
    /// <param name="command">The CreateMaintenanceRequestCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public MaintenanceRequest(CreateMaintenanceRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.VehicleId, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.FaultDescription);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.FaultDescription.Length, 500);
        ArgumentOutOfRangeException.ThrowIfNegative(command.EstimatedCost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.EstimatedCost, 1000000m);

        VehicleId = command.VehicleId;
        MechanicId = command.MechanicId;
        FaultDescription = command.FaultDescription;
        EstimatedCost = command.EstimatedCost;
        Status = command.Status;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this maintenance request.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public int VehicleId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the mechanic user.
    /// </summary>
    public int? MechanicId { get; private set; }

    /// <summary>
    ///     Gets the description of the fault to repair.
    /// </summary>
    public string FaultDescription { get; private set; }

    /// <summary>
    ///     Gets the estimated cost of the repair.
    /// </summary>
    public decimal EstimatedCost { get; private set; }

    /// <summary>
    ///     Gets the current status of the maintenance request.
    /// </summary>
    public MaintenanceRequestStatus Status { get; private set; }

    /// <summary>
    ///     Updates the maintenance request information from an update command.
    /// </summary>
    /// <param name="command">The UpdateMaintenanceRequestCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the update breaks the rule: A completed or rejected maintenance request cannot change its status.</exception>
    public void UpdateInformation(UpdateMaintenanceRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (Status is MaintenanceRequestStatus.Completed or MaintenanceRequestStatus.Rejected && command.Status != Status)
            throw new InvalidOperationException("A completed or rejected maintenance request cannot change its status.");

        ArgumentException.ThrowIfNullOrWhiteSpace(command.FaultDescription);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.FaultDescription.Length, 500);
        ArgumentOutOfRangeException.ThrowIfNegative(command.EstimatedCost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.EstimatedCost, 1000000m);

        MechanicId = command.MechanicId;
        FaultDescription = command.FaultDescription;
        EstimatedCost = command.EstimatedCost;
        Status = command.Status;
    }
}
