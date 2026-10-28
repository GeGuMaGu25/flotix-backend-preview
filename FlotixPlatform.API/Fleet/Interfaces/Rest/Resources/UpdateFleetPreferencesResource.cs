using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Fleet.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update the fleet preferences.
/// </summary>
/// <param name="SpeedLimitKmh">The speed limit that triggers speeding alerts.</param>
/// <param name="MaintenanceIntervalKm">The mileage interval between preventive maintenances.</param>
/// <param name="FuelAnomalyDropPct">The fuel level drop percentage considered an anomaly.</param>
public record UpdateFleetPreferencesResource(
    [Range(1, 300, ErrorMessage = "RangeField")]
    int SpeedLimitKmh,
    [Range(1, 1000000, ErrorMessage = "RangeField")]
    int MaintenanceIntervalKm,
    [Range(1, 100, ErrorMessage = "RangeField")]
    int FuelAnomalyDropPct);
