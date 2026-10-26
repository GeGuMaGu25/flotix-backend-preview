using Flotix.Api.Fleet.Domain;
using Flotix.Api.Iam.Domain;
using Flotix.Api.Maintenance.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Maintenance.Persistence;

public static class MaintenanceModel
{
    public static ModelBuilder ApplyMaintenanceModel(this ModelBuilder model)
    {
        model.Entity<Workshop>().HasOne<User>().WithMany().HasForeignKey(w => w.MechanicId);

        model.Entity<MaintenanceRequest>(request =>
        {
            request.HasOne<Vehicle>().WithMany().HasForeignKey(m => m.VehicleId);
            request.HasOne<User>().WithMany().HasForeignKey(m => m.MechanicId);
        });

        return model;
    }
}
