namespace FlotixPlatform.API.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about the fleet preferences.
/// </summary>
/// <param name="Id">The server-generated ID of the fleet preferences</param>
/// <param name="SpeedLimitKmh">The speed limit that triggers speeding alerts.</param>
/// <param name="MaintenanceIntervalKm">The mileage interval between preventive maintenances.</param>
/// <param name="FuelAnomalyDropPct">The fuel level drop percentage considered an anomaly.</param>
public record FleetPreferencesResource(
    int Id,
    int SpeedLimitKmh,
    int MaintenanceIntervalKm,
    int FuelAnomalyDropPct);
