using FlotixPlatform.API.Fuel.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Fuel.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Fuel Control bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Fuel Control bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyFuelConfiguration(this ModelBuilder builder)
    {
        builder.Entity<FuelRecord>().HasKey(f => f.Id);
        builder.Entity<FuelRecord>().Property(f => f.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<FuelRecord>().Property(f => f.VehicleId).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.DriverId).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.Liters).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.Cost).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.MileageAtRefill).IsRequired();
        builder.Entity<FuelRecord>().Property(f => f.RecordedDate).IsRequired();
        builder.Entity<FuelRecord>().HasIndex(f => f.VehicleId);
        builder.Entity<FuelRecord>().HasIndex(f => f.DriverId);
    }
}
