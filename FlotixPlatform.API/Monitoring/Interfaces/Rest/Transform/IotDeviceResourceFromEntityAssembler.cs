using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a IotDeviceResource from a IotDevice.
/// </summary>
public static class IotDeviceResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a IotDeviceResource from a IotDevice.
    /// </summary>
    /// <param name="entity">The IotDevice entity</param>
    /// <returns>
    ///     A IotDeviceResource assembled from the IotDevice
    /// </returns>
    public static IotDeviceResource ToResourceFromEntity(IotDevice entity) =>
        new(entity.Id, entity.VehicleId, entity.SerialNumber.Value, entity.IsConnected);
}
