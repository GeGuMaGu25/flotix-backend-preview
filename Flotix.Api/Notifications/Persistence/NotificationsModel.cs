using Flotix.Api.Iam.Domain;
using Flotix.Api.Monitoring.Domain;
using Flotix.Api.Notifications.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Notifications.Persistence;

public static class NotificationsModel
{
    public static ModelBuilder ApplyNotificationsModel(this ModelBuilder model)
    {
        model.Entity<Notification>(notification =>
        {
            notification.HasOne<User>().WithMany().HasForeignKey(n => n.UserId);
            notification.HasOne<Alert>().WithMany().HasForeignKey(n => n.AlertId);
        });

        model.Entity<NotificationPreference>(preference =>
        {
            preference.HasIndex(p => p.UserId).IsUnique();
            preference.HasOne<User>().WithMany().HasForeignKey(p => p.UserId);
        });

        return model;
    }
}
