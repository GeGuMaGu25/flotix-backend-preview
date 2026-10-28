using FlotixPlatform.API.Fleet.Domain.Model.Commands;
using FlotixPlatform.API.Fleet.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateFleetPreferencesCommand from an UpdateFleetPreferencesResource.
/// </summary>
public static class UpdateFleetPreferencesCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateFleetPreferencesCommand from an UpdateFleetPreferencesResource.
    /// </summary>
    /// <param name="resource">The UpdateFleetPreferencesResource resource</param>
    /// <returns>
    ///     An UpdateFleetPreferencesCommand assembled from the UpdateFleetPreferencesResource
    /// </returns>
    public static UpdateFleetPreferencesCommand ToCommandFromResource(UpdateFleetPreferencesResource resource) =>
        new(resource.SpeedLimitKmh, resource.MaintenanceIntervalKm, resource.FuelAnomalyDropPct);
}
