using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a FleetPreferencesResource from a FleetPreferences.
/// </summary>
public static class FleetPreferencesResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a FleetPreferencesResource from a FleetPreferences.
    /// </summary>
    /// <param name="entity">The FleetPreferences entity</param>
    /// <returns>
    ///     A FleetPreferencesResource assembled from the FleetPreferences
    /// </returns>
    public static FleetPreferencesResource ToResourceFromEntity(FleetPreferences entity) =>
        new(entity.Id, entity.SpeedLimitKmh, entity.MaintenanceIntervalKm, entity.FuelAnomalyDropPct);
}
