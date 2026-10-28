using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Real-Time Monitoring / Fleet Tracking bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>monitoring</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Real-Time Monitoring / Fleet Tracking bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        builder.Entity<IotDevice>().ToTable(nameof(IotDevice), "monitoring");
        builder.Entity<IotDevice>().HasKey(i => i.Id);
        builder.Entity<IotDevice>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IotDevice>().Property(i => i.VehicleId).IsRequired();
        builder.Entity<IotDevice>()
            .Property(i => i.SerialNumber)
            .HasConversion(valueObject => valueObject.Value, value => new SerialNumber(value))
            .HasMaxLength(100)
            .IsRequired();
        builder.Entity<IotDevice>().Property(i => i.IsConnected).IsRequired();
        builder.Entity<IotDevice>()
            .HasOne<Vehicle>()
            .WithMany()
            .HasForeignKey(i => i.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<IotDevice>().HasIndex(i => i.VehicleId).IsUnique();
        builder.Entity<IotDevice>().HasIndex(i => i.SerialNumber).IsUnique();

        builder.Entity<TelemetryData>().ToTable(nameof(TelemetryData), "monitoring");
        builder.Entity<TelemetryData>().HasKey(t => t.Id);
        builder.Entity<TelemetryData>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<TelemetryData>().Property(t => t.DeviceId).IsRequired();
        builder.Entity<TelemetryData>().Property(t => t.Latitude).IsRequired();
        builder.Entity<TelemetryData>().Property(t => t.Longitude).IsRequired();
        builder.Entity<TelemetryData>().Property(t => t.Speed).IsRequired();
        builder.Entity<TelemetryData>().Property(t => t.RecordedAt).IsRequired();
        builder.Entity<TelemetryData>()
            .HasOne<IotDevice>()
            .WithMany()
            .HasForeignKey(t => t.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<TelemetryData>().HasIndex(t => t.DeviceId);
    }
}
