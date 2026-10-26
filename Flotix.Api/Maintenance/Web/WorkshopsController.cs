using System.ComponentModel.DataAnnotations;
using Flotix.Api.Maintenance.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Maintenance.Web;

public sealed record WorkshopRequest(
    string MechanicId,
    [StringLength(80, MinimumLength = 1)] string City,
    [StringLength(80, MinimumLength = 1)] string District,
    [StringLength(200, MinimumLength = 1)] string Address,
    [Phone, StringLength(30)] string Phone,
    [Range(0, 5)] decimal Rating = 0,
    [MaxLength(20)] List<string>? Tags = null) : IResourceRequest<WorkshopRequest, Workshop>
{
    public Workshop ToEntity() => new(MechanicId, City, District, Address, Phone, Rating, Tags ?? []);

    public void ApplyTo(Workshop workshop) => workshop.Update(City, District, Address, Phone, Rating, Tags ?? []);

    public static WorkshopRequest FromEntity(Workshop workshop)
        => new(workshop.MechanicId, workshop.City, workshop.District, workshop.Address, workshop.Phone, workshop.Rating, workshop.Tags);
}

public sealed class WorkshopsController(IRepository<Workshop> workshops, IUnitOfWork unitOfWork)
    : CrudController<Workshop, WorkshopRequest>(workshops, unitOfWork);
