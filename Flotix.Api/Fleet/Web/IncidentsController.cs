using System.ComponentModel.DataAnnotations;
using Flotix.Api.Fleet.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Fleet.Web;

public sealed record IncidentRequest(
    string VehicleId,
    string DriverId,
    [StringLength(500, MinimumLength = 1)] string Description,
    IncidentStatus Status = IncidentStatus.Open) : IResourceRequest<IncidentRequest, Incident>
{
    public Incident ToEntity() => new(VehicleId, DriverId, Description, Status);

    public void ApplyTo(Incident incident) => incident.Update(Description, Status);

    public static IncidentRequest FromEntity(Incident incident)
        => new(incident.VehicleId, incident.DriverId, incident.Description, incident.Status);
}

public sealed class IncidentsController(IRepository<Incident> incidents, IUnitOfWork unitOfWork)
    : CrudController<Incident, IncidentRequest>(incidents, unitOfWork);
