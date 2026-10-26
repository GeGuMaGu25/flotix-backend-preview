using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Fleet.Domain;

/// <summary>Fleet-wide thresholds (singleton aggregate).</summary>
public sealed class FleetPreferences : Entity
{
    public const string DefaultId = "default";

    private FleetPreferences() : base(DefaultId) { }

    public int SpeedLimitKmh { get; private set; }

    public int MaintenanceIntervalKm { get; private set; }

    public int FuelAnomalyDropPct { get; private set; }

    public void Update(int speedLimitKmh, int maintenanceIntervalKm, int fuelAnomalyDropPct)
    {
        SpeedLimitKmh = speedLimitKmh;
        MaintenanceIntervalKm = maintenanceIntervalKm;
        FuelAnomalyDropPct = fuelAnomalyDropPct;
    }
}
