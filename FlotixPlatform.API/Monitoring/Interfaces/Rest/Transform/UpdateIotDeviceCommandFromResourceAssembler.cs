using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateIotDeviceCommand from an UpdateIotDeviceResource.
/// </summary>
public static class UpdateIotDeviceCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateIotDeviceCommand from an UpdateIotDeviceResource.
    /// </summary>
    /// <param name="iotDeviceId">The identifier of the IoT device to update</param>
    /// <param name="resource">The UpdateIotDeviceResource resource</param>
    /// <returns>
    ///     An UpdateIotDeviceCommand assembled from the UpdateIotDeviceResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateIotDeviceCommand ToCommandFromResource(int iotDeviceId, UpdateIotDeviceResource resource) =>
        new(iotDeviceId, resource.VehicleId, resource.ConnectionStatus);
}
