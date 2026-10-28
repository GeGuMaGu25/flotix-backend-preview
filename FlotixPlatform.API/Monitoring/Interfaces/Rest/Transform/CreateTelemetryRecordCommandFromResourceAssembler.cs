using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateTelemetryRecordCommand from a CreateTelemetryRecordResource.
/// </summary>
public static class CreateTelemetryRecordCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateTelemetryRecordCommand from a CreateTelemetryRecordResource.
    /// </summary>
    /// <param name="resource">The CreateTelemetryRecordResource resource</param>
    /// <returns>
    ///     A CreateTelemetryRecordCommand assembled from the CreateTelemetryRecordResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateTelemetryRecordCommand ToCommandFromResource(CreateTelemetryRecordResource resource) =>
        new(resource.DeviceId, resource.Latitude, resource.Longitude, resource.Speed, resource.Timestamp);
}
