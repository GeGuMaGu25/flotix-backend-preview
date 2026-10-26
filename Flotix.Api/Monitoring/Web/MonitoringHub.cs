using Flotix.Api.Monitoring.Domain;
using Microsoft.AspNetCore.SignalR;

namespace Flotix.Api.Monitoring.Web;

/// <summary>Server-to-client events of the real-time fleet monitoring.</summary>
public interface IMonitoringClient
{
    Task TelemetryReceived(TelemetryData telemetry);

    Task AlertActivated(Alert alert);
}

/// <summary>Real-time channel at <c>/hubs/monitoring</c>; clients only listen.</summary>
public sealed class MonitoringHub : Hub<IMonitoringClient>;
