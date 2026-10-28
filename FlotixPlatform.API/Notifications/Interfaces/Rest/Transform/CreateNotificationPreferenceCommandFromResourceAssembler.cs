using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateNotificationPreferenceCommand from a CreateNotificationPreferenceResource.
/// </summary>
public static class CreateNotificationPreferenceCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateNotificationPreferenceCommand from a CreateNotificationPreferenceResource.
    /// </summary>
    /// <param name="resource">The CreateNotificationPreferenceResource resource</param>
    /// <returns>
    ///     A CreateNotificationPreferenceCommand assembled from the CreateNotificationPreferenceResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateNotificationPreferenceCommand ToCommandFromResource(CreateNotificationPreferenceResource resource) =>
        new(resource.UserId, resource.Maintenance, resource.Speed, resource.Incidents);
}
