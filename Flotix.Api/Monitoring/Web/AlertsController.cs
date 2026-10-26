using System.ComponentModel.DataAnnotations;
using Flotix.Api.Monitoring.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;
using Microsoft.AspNetCore.SignalR;

namespace Flotix.Api.Monitoring.Web;

public sealed record AlertRequest(string VehicleId, [StringLength(120, MinimumLength = 1)] string AlertType)
    : IResourceRequest<AlertRequest, Alert>
{
    public Alert ToEntity() => new(VehicleId, AlertType);

    public void ApplyTo(Alert alert) => throw new DomainException("Alerts cannot be modified.");

    public static AlertRequest FromEntity(Alert alert) => new(alert.VehicleId, alert.AlertType);
}

public sealed class AlertsController(IRepository<Alert> alerts, IUnitOfWork unitOfWork, IHubContext<MonitoringHub, IMonitoringClient> hub)
    : CrudController<Alert, AlertRequest>(alerts, unitOfWork)
{
    protected override Task OnCreatedAsync(Alert entity, CancellationToken cancellationToken)
        => hub.Clients.All.AlertActivated(entity);
}
