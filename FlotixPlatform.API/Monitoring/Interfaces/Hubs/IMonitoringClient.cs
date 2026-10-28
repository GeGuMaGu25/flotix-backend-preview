using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Hubs;

/// <summary>
///     Defines the server-to-client events of the <see cref="MonitoringHub"/>.
/// </summary>
public interface IMonitoringClient
{
    /// <summary>
    ///     Receives a telemetry record as soon as it is created; sent to every connected client.
    /// </summary>
    /// <param name="telemetryRecord">The created telemetry record.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task TelemetryRecordReceived(TelemetryRecordResource telemetryRecord);
}
