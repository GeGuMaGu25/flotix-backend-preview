using FlotixPlatform.API.Fleet.Domain.Model.Commands;

namespace FlotixPlatform.API.Fleet.Domain.Model.Aggregates;

/// <summary>
///     Represents the fleet-wide thresholds used to raise alerts and plan maintenance.
/// </summary>
/// <remarks>
///     This aggregate root is a singleton: a single instance identified by <see cref="DefaultId"/>
///     is seeded by the persistence layer and only updated through an UpdateFleetPreferencesCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class FleetPreferences
{
    /// <summary>
    ///     The identifier of the single fleet preferences instance.
    /// </summary>
    public const int DefaultId = 1;

    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected FleetPreferences()
    {
    }

    /// <summary>
    ///     Gets the server-generated identifier for these fleet preferences.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the speed limit that triggers speeding alerts.
    /// </summary>
    public int SpeedLimitKmh { get; private set; }

    /// <summary>
    ///     Gets the mileage interval between preventive maintenances.
    /// </summary>
    public int MaintenanceIntervalKm { get; private set; }

    /// <summary>
    ///     Gets the fuel level drop percentage considered an anomaly.
    /// </summary>
    public int FuelAnomalyDropPct { get; private set; }

    /// <summary>
    ///     Updates the fleet preferences from an update command.
    /// </summary>
    /// <param name="command">The UpdateFleetPreferencesCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateFleetPreferencesCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.SpeedLimitKmh);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.MaintenanceIntervalKm);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(command.FuelAnomalyDropPct);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.FuelAnomalyDropPct, 100);

        SpeedLimitKmh = command.SpeedLimitKmh;
        MaintenanceIntervalKm = command.MaintenanceIntervalKm;
        FuelAnomalyDropPct = command.FuelAnomalyDropPct;
    }
}
