namespace FlotixPlatform.API.Fleet.Domain.Model.Commands;

/// <summary>
///     Command to update the fleet preferences
/// </summary>
/// <param name="SpeedLimitKmh">The speed limit that triggers speeding alerts.</param>
/// <param name="MaintenanceIntervalKm">The mileage interval between preventive maintenances.</param>
/// <param name="FuelAnomalyDropPct">The fuel level drop percentage considered an anomaly.</param>
public record UpdateFleetPreferencesCommand(int SpeedLimitKmh, int MaintenanceIntervalKm, int FuelAnomalyDropPct);
