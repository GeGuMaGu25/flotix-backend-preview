using Flotix.Api.Fleet.Domain;
using Flotix.Api.Iam.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Fleet.Persistence;

public static class FleetModel
{
    public static ModelBuilder ApplyFleetModel(this ModelBuilder model)
    {
        model.Entity<Vehicle>(vehicle =>
        {
            vehicle.HasIndex(v => v.Plate).IsUnique();
            vehicle.HasOne<User>().WithMany().HasForeignKey(v => v.OwnerId);
            vehicle.HasOne<User>().WithMany().HasForeignKey(v => v.DriverId);
        });

        model.Entity<FuelRecord>(record =>
        {
            record.HasOne<Vehicle>().WithMany().HasForeignKey(f => f.VehicleId);
            record.HasOne<User>().WithMany().HasForeignKey(f => f.DriverId);
        });

        model.Entity<Incident>(incident =>
        {
            incident.HasOne<Vehicle>().WithMany().HasForeignKey(i => i.VehicleId);
            incident.HasOne<User>().WithMany().HasForeignKey(i => i.DriverId);
        });

        model.Entity<FleetPreferences>().HasData(new
        {
            Id = FleetPreferences.DefaultId,
            CreatedAt = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
            SpeedLimitKmh = 90,
            MaintenanceIntervalKm = 10000,
            FuelAnomalyDropPct = 30
        });

        return model;
    }
}
