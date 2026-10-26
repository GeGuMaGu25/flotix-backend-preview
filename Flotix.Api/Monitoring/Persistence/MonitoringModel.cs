using Flotix.Api.Fleet.Domain;
using Flotix.Api.Monitoring.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Monitoring.Persistence;

public static class MonitoringModel
{
    public static ModelBuilder ApplyMonitoringModel(this ModelBuilder model)
    {
        model.Entity<IotDevice>(device =>
        {
            device.HasIndex(d => d.SerialNumber).IsUnique();
            device.HasOne<Vehicle>().WithMany().HasForeignKey(d => d.VehicleId);
        });

        model.Entity<TelemetryData>().HasOne<IotDevice>().WithMany().HasForeignKey(t => t.DeviceId);

        model.Entity<Alert>().HasOne<Vehicle>().WithMany().HasForeignKey(a => a.VehicleId);

        return model;
    }
}
