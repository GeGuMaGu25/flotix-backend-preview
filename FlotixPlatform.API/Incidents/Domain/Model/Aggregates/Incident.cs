using FlotixPlatform.API.Incidents.Domain.Model.Commands;
using FlotixPlatform.API.Incidents.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Incidents.Domain.Model.Aggregates;

/// <summary>
///     Represents an incident reported by a driver about a vehicle.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateIncidentCommand and updated through an UpdateIncidentCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class Incident
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Incident()
    {
        Description = string.Empty;
    }

    /// <summary>
    ///     Creates a new Incident from a creation command.
    /// </summary>
    /// <param name="command">The CreateIncidentCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public Incident(CreateIncidentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.VehicleId, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.DriverId, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Description);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Description.Length, 500);

        VehicleId = command.VehicleId;
        DriverId = command.DriverId;
        Description = command.Description;
        Status = command.Status;
        ReportedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this incident.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the vehicle.
    /// </summary>
    public int VehicleId { get; private set; }

    /// <summary>
    ///     Gets the identifier of the driver user.
    /// </summary>
    public int DriverId { get; private set; }

    /// <summary>
    ///     Gets the description of the incident.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    ///     Gets the current status of the incident.
    /// </summary>
    public IncidentStatus Status { get; private set; }

    /// <summary>
    ///     Gets the moment the incident was reported.
    /// </summary>
    public DateTimeOffset ReportedAt { get; private set; }

    /// <summary>
    ///     Updates the incident information from an update command.
    /// </summary>
    /// <param name="command">The UpdateIncidentCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateIncidentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Description);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Description.Length, 500);

        Description = command.Description;
        Status = command.Status;
    }
}
