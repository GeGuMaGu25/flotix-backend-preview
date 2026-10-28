using FlotixPlatform.API.Incidents.Domain.Model.Aggregates;
using FlotixPlatform.API.Incidents.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Incident Management bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Incident Management bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyIncidentsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Incident>().HasKey(i => i.Id);
        builder.Entity<Incident>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Incident>().Property(i => i.VehicleId).IsRequired();
        builder.Entity<Incident>().Property(i => i.DriverId).IsRequired();
        builder.Entity<Incident>().Property(i => i.Description).IsRequired().HasMaxLength(500);
        builder.Entity<Incident>().Property(i => i.Status).HasConversion<string>().HasMaxLength(30).IsRequired();
        builder.Entity<Incident>().Property(i => i.ReportedAt).IsRequired();
        builder.Entity<Incident>().HasIndex(i => i.VehicleId);
        builder.Entity<Incident>().HasIndex(i => i.DriverId);
    }
}
