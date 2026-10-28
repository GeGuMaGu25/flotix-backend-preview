using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Hubs;

/// <summary>
///     Defines the server-to-client events of the <see cref="MonitoringHub"/>.
/// </summary>
public interface IMonitoringClient
{
    /// <summary>
    ///     Receives the latest location of a vehicle as soon as a telemetry packet is processed; sent to every
    ///     connected client.
    /// </summary>
    /// <param name="vehicleLocation">The updated location of the vehicle.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task VehicleLocationUpdated(VehicleLocationResource vehicleLocation);
}
