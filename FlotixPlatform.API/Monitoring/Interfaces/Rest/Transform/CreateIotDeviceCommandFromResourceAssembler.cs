using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateIotDeviceCommand from a CreateIotDeviceResource.
/// </summary>
public static class CreateIotDeviceCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateIotDeviceCommand from a CreateIotDeviceResource.
    /// </summary>
    /// <param name="resource">The CreateIotDeviceResource resource</param>
    /// <returns>
    ///     A CreateIotDeviceCommand assembled from the CreateIotDeviceResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateIotDeviceCommand ToCommandFromResource(CreateIotDeviceResource resource) =>
        new(resource.VehicleId, new SerialNumber(resource.SerialNumber), resource.ConnectionStatus);
}
