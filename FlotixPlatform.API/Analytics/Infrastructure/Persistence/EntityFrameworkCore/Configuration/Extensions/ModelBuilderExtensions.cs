using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Reporting & Analytics bounded context.
/// </summary>
/// <remarks>
///     The tables live in the <c>analytics</c> schema and reference other bounded contexts through
///     foreign keys with restricted deletes, as defined in the database design of the project report.
/// </remarks>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Reporting & Analytics bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Report>().ToTable(nameof(Report), "analytics");
        builder.Entity<Report>().HasKey(r => r.Id);
        builder.Entity<Report>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Report>().Property(r => r.OwnerId).IsRequired();
        builder.Entity<Report>().Property(r => r.ReportType).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Entity<Report>().Property(r => r.GeneratedDate).IsRequired();
        builder.Entity<Report>().Property(r => r.FilterParams).IsRequired();
        builder.Entity<Report>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Report>().HasIndex(r => r.OwnerId);
    }
}
