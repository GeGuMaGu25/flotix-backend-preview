using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateTelemetryDataCommand from a CreateTelemetryDataResource.
/// </summary>
public static class CreateTelemetryDataCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateTelemetryDataCommand from a CreateTelemetryDataResource.
    /// </summary>
    /// <param name="resource">The CreateTelemetryDataResource resource</param>
    /// <returns>
    ///     A CreateTelemetryDataCommand assembled from the CreateTelemetryDataResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateTelemetryDataCommand ToCommandFromResource(CreateTelemetryDataResource resource) =>
        new(resource.DeviceId, resource.Latitude, resource.Longitude, resource.Speed, resource.RecordedAt);
}
