using FlotixPlatform.API.Monitoring.Application.Internal.OutboundServices;
using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Interfaces.Hubs;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.SignalR;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Realtime.SignalR.Services;

/// <summary>
///     Publishes the updated location of a vehicle to the clients of the <see cref="MonitoringHub"/>.
/// </summary>
/// <param name="monitoringHub">The SignalR monitoring hub context.</param>
public class VehicleLocationNotifier(IHubContext<MonitoringHub, IMonitoringClient> monitoringHub) : IVehicleLocationNotifier
{
    /// <inheritdoc />
    public Task NotifyAsync(Guid vehicleId, TelemetryData telemetryData, CancellationToken cancellationToken = default) =>
        monitoringHub.Clients.All.VehicleLocationUpdated(
            VehicleLocationResourceFromEntityAssembler.ToResourceFromEntity(vehicleId, telemetryData));
}
