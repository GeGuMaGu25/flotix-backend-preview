using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Maintenance Management bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Maintenance Management bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyMaintenanceConfiguration(this ModelBuilder builder)
    {
        builder.Entity<MaintenanceRequest>().HasKey(m => m.Id);
        builder.Entity<MaintenanceRequest>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<MaintenanceRequest>().Property(m => m.VehicleId).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.MechanicId).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.Description).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.EstimatedCost).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.RequestDate).IsRequired();
        builder.Entity<MaintenanceRequest>().HasIndex(m => m.VehicleId);
        builder.Entity<MaintenanceRequest>().HasIndex(m => m.MechanicId);
    }
}
