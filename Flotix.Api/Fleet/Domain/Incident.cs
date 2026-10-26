using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Fleet.Domain;

public sealed class Incident : Entity
{
    private Incident() { }

    public Incident(string vehicleId, string driverId, string description, IncidentStatus status)
    {
        VehicleId = vehicleId;
        DriverId = driverId;
        ReportedAt = CreatedAt;
        Update(description, status);
    }

    public string VehicleId { get; private set; } = null!;

    public string DriverId { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public IncidentStatus Status { get; private set; }

    public DateTimeOffset ReportedAt { get; private set; }

    public void Update(string description, IncidentStatus status)
    {
        Description = description;
        Status = status;
    }
}
