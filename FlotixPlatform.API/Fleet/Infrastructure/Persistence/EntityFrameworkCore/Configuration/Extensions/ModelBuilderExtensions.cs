using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Vehicle & Fleet Management bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Vehicle & Fleet Management bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyFleetConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Vehicle>().HasKey(v => v.Id);
        builder.Entity<Vehicle>().Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Vehicle>().Property(v => v.OwnerId).IsRequired();
        builder.Entity<Vehicle>()
            .Property(v => v.LicensePlate)
            .HasConversion(valueObject => valueObject.Value, value => new LicensePlate(value))
            .HasMaxLength(15)
            .IsRequired();
        builder.Entity<Vehicle>().Property(v => v.VehicleType).IsRequired().HasMaxLength(50);
        builder.Entity<Vehicle>().Property(v => v.CurrentMileage).IsRequired();
        builder.Entity<Vehicle>().Property(v => v.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<Vehicle>().HasIndex(v => v.OwnerId);
        builder.Entity<Vehicle>().HasIndex(v => v.DriverId);
        builder.Entity<Vehicle>().HasIndex(v => v.LicensePlate).IsUnique();
    }
}
