using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a ReceiveTelemetryDataCommand from a ReceiveTelemetryDataResource.
/// </summary>
public static class ReceiveTelemetryDataCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a ReceiveTelemetryDataCommand from a ReceiveTelemetryDataResource.
    /// </summary>
    /// <param name="resource">The ReceiveTelemetryDataResource resource</param>
    /// <returns>
    ///     A ReceiveTelemetryDataCommand assembled from the ReceiveTelemetryDataResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a required field of the packet is missing.</exception>
    public static ReceiveTelemetryDataCommand ToCommandFromResource(ReceiveTelemetryDataResource resource) =>
        new(Required(resource.DeviceId, nameof(resource.DeviceId)),
            Required(resource.Latitude, nameof(resource.Latitude)),
            Required(resource.Longitude, nameof(resource.Longitude)),
            Required(resource.Speed, nameof(resource.Speed)),
            Required(resource.RecordedAt, nameof(resource.RecordedAt)));

    private static T Required<T>(T? value, string name) where T : struct =>
        value ?? throw new ArgumentException($"The telemetry packet has no {name}.", name);
}
