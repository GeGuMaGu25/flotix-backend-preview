using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateNotificationCommand from an UpdateNotificationResource.
/// </summary>
public static class UpdateNotificationCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateNotificationCommand from an UpdateNotificationResource.
    /// </summary>
    /// <param name="notificationId">The identifier of the notification to update</param>
    /// <param name="resource">The UpdateNotificationResource resource</param>
    /// <returns>
    ///     An UpdateNotificationCommand assembled from the UpdateNotificationResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateNotificationCommand ToCommandFromResource(int notificationId, UpdateNotificationResource resource) =>
        new(notificationId, resource.IsRead);
}
