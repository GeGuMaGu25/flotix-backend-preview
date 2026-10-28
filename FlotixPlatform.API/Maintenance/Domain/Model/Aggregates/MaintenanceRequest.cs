using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;

/// <summary>
///     Represents a request to repair a vehicle, attended by a mechanic.
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
        Description = string.Empty;
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
        ArgumentOutOfRangeException.ThrowIfEqual(command.VehicleId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(command.MechanicId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Description);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Description.Length, 500);

        VehicleId = command.VehicleId;
        MechanicId = command.MechanicId;
        Description = command.Description;
        EstimatedCost = 0m;
        Status = MaintenanceRequestStatus.Pending;
        RequestDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this maintenance request.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public Guid VehicleId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the mechanic user.
    /// </summary>
    public Guid MechanicId { get; private set; }

    /// <summary>
    ///     Gets the description of the fault to repair.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    ///     Gets the estimated cost quoted by the workshop.
    /// </summary>
    public decimal EstimatedCost { get; private set; }

    /// <summary>
    ///     Gets the current status of the maintenance request.
    /// </summary>
    public MaintenanceRequestStatus Status { get; private set; }

    /// <summary>
    ///     Gets the moment the maintenance was requested.
    /// </summary>
    public DateTimeOffset RequestDate { get; private set; }

    /// <summary>
    ///     Creates a new MaintenanceRequest, in "Pendiente" status, from a submit command.
    /// </summary>
    /// <param name="command">The SubmitMaintenanceRequestCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public MaintenanceRequest(SubmitMaintenanceRequestCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfEqual(command.VehicleId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(command.MechanicId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Description);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Description.Length, 500);

        VehicleId = command.VehicleId;
        MechanicId = command.MechanicId;
        Description = command.Description;
        EstimatedCost = 0m;
        Status = MaintenanceRequestStatus.Pending;
        RequestDate = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Registers the diagnostic and quote of the workshop: "Pendiente" → "Presupuestado".
    /// </summary>
    /// <param name="amount">The estimated cost quoted by the workshop.</param>
    /// <exception cref="ArgumentException">Thrown when the amount is negative or too high.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the request is not pending.</exception>
    public void SubmitQuote(decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(amount, 1000000m);
        TransitionTo(MaintenanceRequestStatus.Quoted, MaintenanceRequestStatus.Pending);
        EstimatedCost = amount;
    }

    /// <summary>
    ///     Approves the quoted budget: "Presupuestado" → "Aprobado".
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the request has not been quoted.</exception>
    public void Approve() => TransitionTo(MaintenanceRequestStatus.Approved, MaintenanceRequestStatus.Quoted);

    /// <summary>
    ///     Rejects the quoted budget: "Presupuestado" → "Rechazado".
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the request has not been quoted.</exception>
    public void Reject() => TransitionTo(MaintenanceRequestStatus.Rejected, MaintenanceRequestStatus.Quoted);

    /// <summary>
    ///     Starts the approved repair: "Aprobado" → "En reparación".
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the budget has not been approved.</exception>
    public void StartRepair() => TransitionTo(MaintenanceRequestStatus.InRepair, MaintenanceRequestStatus.Approved);

    /// <summary>
    ///     Finishes the repair: "En reparación" → "Completado".
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the repair has not started.</exception>
    public void Finish() => TransitionTo(MaintenanceRequestStatus.Completed, MaintenanceRequestStatus.InRepair);

    /// <summary>
    ///     Moves the request to a new status when it is in the status the transition requires.
    /// </summary>
    /// <remarks>
    ///     Completed and rejected requests are closed, so no transition starts from them.
    /// </remarks>
    /// <param name="newStatus">The status after the transition.</param>
    /// <param name="requiredStatus">The status the request must have.</param>
    /// <exception cref="InvalidOperationException">Thrown when the request is not in the required status.</exception>
    private void TransitionTo(MaintenanceRequestStatus newStatus, MaintenanceRequestStatus requiredStatus)
    {
        if (Status != requiredStatus)
            throw new InvalidOperationException(
                $"A maintenance request in status {Status} cannot change to {newStatus}; it must be {requiredStatus}.");

        Status = newStatus;
    }

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

        ArgumentOutOfRangeException.ThrowIfEqual(command.MechanicId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Description);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Description.Length, 500);
        ArgumentOutOfRangeException.ThrowIfNegative(command.EstimatedCost);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.EstimatedCost, 1000000m);

        MechanicId = command.MechanicId;
        Description = command.Description;
        EstimatedCost = command.EstimatedCost;
        Status = command.Status;
    }
}
