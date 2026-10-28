using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a NotificationResource from a Notification.
/// </summary>
public static class NotificationResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a NotificationResource from a Notification.
    /// </summary>
    /// <param name="entity">The Notification entity</param>
    /// <returns>
    ///     A NotificationResource assembled from the Notification
    /// </returns>
    public static NotificationResource ToResourceFromEntity(Notification entity) =>
        new(entity.Id, entity.UserId, entity.AlertId, entity.Message, entity.IsRead, entity.SentDate);
}
