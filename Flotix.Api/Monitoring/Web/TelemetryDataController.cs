using System.ComponentModel.DataAnnotations;
using Flotix.Api.Monitoring.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;
using Microsoft.AspNetCore.SignalR;

namespace Flotix.Api.Monitoring.Web;

/// <remarks>Readings are immutable facts reported by the device.</remarks>
public sealed record TelemetryDataRequest(
    string DeviceId,
    [Range(-90, 90)] double Latitude,
    [Range(-180, 180)] double Longitude,
    [Range(0, 400)] int Speed,
    DateTimeOffset Timestamp) : IResourceRequest<TelemetryDataRequest, TelemetryData>
{
    public TelemetryData ToEntity() => new(DeviceId, Latitude, Longitude, Speed, Timestamp);

    public void ApplyTo(TelemetryData telemetry) => throw new DomainException("Telemetry readings cannot be modified.");

    public static TelemetryDataRequest FromEntity(TelemetryData telemetry)
        => new(telemetry.DeviceId, telemetry.Latitude, telemetry.Longitude, telemetry.Speed, telemetry.Timestamp);
}

public sealed class TelemetryDataController(IRepository<TelemetryData> telemetry, IUnitOfWork unitOfWork, IHubContext<MonitoringHub, IMonitoringClient> hub)
    : CrudController<TelemetryData, TelemetryDataRequest>(telemetry, unitOfWork)
{
    protected override Task OnCreatedAsync(TelemetryData entity, CancellationToken cancellationToken)
        => hub.Clients.All.TelemetryReceived(entity);
}
