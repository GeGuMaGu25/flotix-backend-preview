using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateNotificationPreferenceCommand from an UpdateNotificationPreferenceResource.
/// </summary>
public static class UpdateNotificationPreferenceCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateNotificationPreferenceCommand from an UpdateNotificationPreferenceResource.
    /// </summary>
    /// <param name="notificationPreferenceId">The identifier of the notification preference to update</param>
    /// <param name="resource">The UpdateNotificationPreferenceResource resource</param>
    /// <returns>
    ///     An UpdateNotificationPreferenceCommand assembled from the UpdateNotificationPreferenceResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateNotificationPreferenceCommand ToCommandFromResource(int notificationPreferenceId, UpdateNotificationPreferenceResource resource) =>
        new(notificationPreferenceId, resource.Maintenance, resource.Speed, resource.Incidents);
}
