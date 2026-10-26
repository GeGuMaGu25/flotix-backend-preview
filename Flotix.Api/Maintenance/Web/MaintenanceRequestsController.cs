using System.ComponentModel.DataAnnotations;
using Flotix.Api.Maintenance.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Maintenance.Web;

public sealed record MaintenanceRequestRequest(
    string VehicleId,
    [StringLength(500, MinimumLength = 1)] string FaultDescription,
    [Range(0, 1000000)] decimal EstimatedCost = 0,
    MaintenanceStatus Status = MaintenanceStatus.Pending,
    string? MechanicId = null) : IResourceRequest<MaintenanceRequestRequest, MaintenanceRequest>
{
    public MaintenanceRequest ToEntity() => new(VehicleId, MechanicId, FaultDescription, EstimatedCost, Status);

    public void ApplyTo(MaintenanceRequest request) => request.Update(MechanicId, FaultDescription, EstimatedCost, Status);

    public static MaintenanceRequestRequest FromEntity(MaintenanceRequest request)
        => new(request.VehicleId, request.FaultDescription, request.EstimatedCost, request.Status, request.MechanicId);
}

public sealed class MaintenanceRequestsController(IRepository<MaintenanceRequest> requests, IUnitOfWork unitOfWork)
    : CrudController<MaintenanceRequest, MaintenanceRequestRequest>(requests, unitOfWork);
