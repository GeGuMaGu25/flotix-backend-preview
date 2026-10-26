using System.ComponentModel.DataAnnotations;
using Flotix.Api.Notifications.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;
using Microsoft.AspNetCore.SignalR;

namespace Flotix.Api.Notifications.Web;

/// <remarks>Only the read flag of an existing notification can change.</remarks>
public sealed record NotificationRequest(
    string UserId,
    [StringLength(500, MinimumLength = 1)] string Message,
    bool IsRead = false,
    string? AlertId = null) : IResourceRequest<NotificationRequest, Notification>
{
    public Notification ToEntity() => new(UserId, AlertId, Message);

    public void ApplyTo(Notification notification) => notification.MarkAs(IsRead);

    public static NotificationRequest FromEntity(Notification notification)
        => new(notification.UserId, notification.Message, notification.IsRead, notification.AlertId);
}

public sealed class NotificationsController(IRepository<Notification> notifications, IUnitOfWork unitOfWork, IHubContext<NotificationsHub, INotificationsClient> hub)
    : CrudController<Notification, NotificationRequest>(notifications, unitOfWork)
{
    protected override Task OnCreatedAsync(Notification entity, CancellationToken cancellationToken)
        => hub.Clients.User(entity.UserId).NotificationReceived(entity);
}
