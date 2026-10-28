using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a NotificationPreferenceResource from a NotificationPreference.
/// </summary>
public static class NotificationPreferenceResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a NotificationPreferenceResource from a NotificationPreference.
    /// </summary>
    /// <param name="entity">The NotificationPreference entity</param>
    /// <returns>
    ///     A NotificationPreferenceResource assembled from the NotificationPreference
    /// </returns>
    public static NotificationPreferenceResource ToResourceFromEntity(NotificationPreference entity) =>
        new(entity.Id, entity.UserId, entity.Maintenance, entity.Speed, entity.Incidents);
}
