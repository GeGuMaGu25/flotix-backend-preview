using Microsoft.AspNetCore.SignalR;

namespace FlotixPlatform.API.Monitoring.Interfaces.Hubs;

/// <summary>
///     SignalR hub of the Real-Time Monitoring / Fleet Tracking bounded context, mapped at <c>/hubs/monitoring</c>.
/// </summary>
/// <remarks>
///     Clients only listen: the IoT Telemetry Processor publishes the location of a vehicle after storing
///     each telemetry packet.
/// </remarks>
public class MonitoringHub : Hub<IMonitoringClient>;
