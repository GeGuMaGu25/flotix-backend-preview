using Microsoft.AspNetCore.SignalR;

namespace FlotixPlatform.API.Monitoring.Interfaces.Hubs;

/// <summary>
///     SignalR hub of the Real-Time Monitoring / Fleet Tracking bounded context, mapped at <c>/hubs/monitoring</c>.
/// </summary>
/// <remarks>
///     Clients only listen: events are pushed by the REST controllers after a successful command.
/// </remarks>
public class MonitoringHub : Hub<IMonitoringClient>;
