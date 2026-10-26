using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Maintenance.Domain;

public sealed class MaintenanceRequest : Entity
{
    private MaintenanceRequest() { }

    public MaintenanceRequest(string vehicleId, string? mechanicId, string faultDescription, decimal estimatedCost, MaintenanceStatus status)
    {
        VehicleId = vehicleId;
        Status = status;
        Update(mechanicId, faultDescription, estimatedCost, status);
    }

    public string VehicleId { get; private set; } = null!;

    public string? MechanicId { get; private set; }

    public string FaultDescription { get; private set; } = null!;

    public decimal EstimatedCost { get; private set; }

    public MaintenanceStatus Status { get; private set; }

    public void Update(string? mechanicId, string faultDescription, decimal estimatedCost, MaintenanceStatus status)
    {
        if (Status is MaintenanceStatus.Completed or MaintenanceStatus.Rejected && status != Status)
            throw new DomainException("A completed or rejected maintenance request cannot be reopened.");

        MechanicId = mechanicId;
        FaultDescription = faultDescription;
        EstimatedCost = estimatedCost;
        Status = status;
    }
}
