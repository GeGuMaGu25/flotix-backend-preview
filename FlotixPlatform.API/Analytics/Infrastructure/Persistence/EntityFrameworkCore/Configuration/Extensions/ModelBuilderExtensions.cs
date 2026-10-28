using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Reporting & Analytics bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Reporting & Analytics bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Report>().HasKey(r => r.Id);
        builder.Entity<Report>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Report>().Property(r => r.OwnerId).IsRequired();
        builder.Entity<Report>().Property(r => r.ReportType).IsRequired().HasMaxLength(120);
        builder.Entity<Report>().Property(r => r.FilterParams).HasColumnType("jsonb").IsRequired();
        builder.Entity<Report>().Property(r => r.GeneratedAt).IsRequired();
        builder.Entity<Report>().HasIndex(r => r.OwnerId);
    }
}
