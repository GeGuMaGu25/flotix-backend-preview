using Flotix.Api.Notifications.Domain;
using Microsoft.AspNetCore.SignalR;

namespace Flotix.Api.Notifications.Web;

public interface INotificationsClient
{
    Task NotificationReceived(Notification notification);
}

/// <summary>
/// Real-time channel at <c>/hubs/notifications</c>. Each notification is pushed only to its recipient,
/// identified by the IAM token (<c>?access_token=</c>), so delivery requires the IAM middleware.
/// </summary>
public sealed class NotificationsHub : Hub<INotificationsClient>;
