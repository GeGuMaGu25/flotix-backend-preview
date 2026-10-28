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
        builder.Entity<Workshop>().HasKey(w => w.Id);
        builder.Entity<Workshop>().Property(w => w.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Workshop>().Property(w => w.MechanicId).IsRequired();
        builder.Entity<Workshop>().Property(w => w.City).IsRequired().HasMaxLength(80);
        builder.Entity<Workshop>().Property(w => w.District).IsRequired().HasMaxLength(80);
        builder.Entity<Workshop>().Property(w => w.Address).IsRequired().HasMaxLength(200);
        builder.Entity<Workshop>()
            .Property(w => w.Phone)
            .HasConversion(valueObject => valueObject.Value, value => new PhoneNumber(value))
            .HasMaxLength(30)
            .IsRequired();
        builder.Entity<Workshop>().Property(w => w.Rating).HasPrecision(12, 2).IsRequired();
        builder.Entity<Workshop>().Property(w => w.Tags).IsRequired();
        builder.Entity<Workshop>().HasIndex(w => w.MechanicId);

        builder.Entity<MaintenanceRequest>().HasKey(m => m.Id);
        builder.Entity<MaintenanceRequest>().Property(m => m.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<MaintenanceRequest>().Property(m => m.VehicleId).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.FaultDescription).IsRequired().HasMaxLength(500);
        builder.Entity<MaintenanceRequest>().Property(m => m.EstimatedCost).HasPrecision(12, 2).IsRequired();
        builder.Entity<MaintenanceRequest>().Property(m => m.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Entity<MaintenanceRequest>().HasIndex(m => m.VehicleId);
        builder.Entity<MaintenanceRequest>().HasIndex(m => m.MechanicId);
    }
}
