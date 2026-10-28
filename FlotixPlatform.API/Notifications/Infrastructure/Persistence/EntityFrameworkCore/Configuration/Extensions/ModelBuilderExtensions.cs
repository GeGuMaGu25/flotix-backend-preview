using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration of the Alerts & Notifications bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies the entity configuration of the Alerts & Notifications bounded context.
    /// </summary>
    /// <param name="builder">The model builder of the application database context.</param>
    public static void ApplyNotificationsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.VehicleId).IsRequired();
        builder.Entity<Alert>().Property(a => a.AlertType).IsRequired().HasMaxLength(120);
        builder.Entity<Alert>().Property(a => a.ActivatedAt).IsRequired();
        builder.Entity<Alert>().HasIndex(a => a.VehicleId);

        builder.Entity<Notification>().HasKey(n => n.Id);
        builder.Entity<Notification>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Notification>().Property(n => n.UserId).IsRequired();
        builder.Entity<Notification>().Property(n => n.Message).IsRequired().HasMaxLength(500);
        builder.Entity<Notification>().Property(n => n.IsRead).IsRequired();
        builder.Entity<Notification>().Property(n => n.SentAt).IsRequired();
        builder.Entity<Notification>().HasIndex(n => n.UserId);
    }
}
