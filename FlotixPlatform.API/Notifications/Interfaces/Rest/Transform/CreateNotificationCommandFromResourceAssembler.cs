using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateNotificationCommand from a CreateNotificationResource.
/// </summary>
public static class CreateNotificationCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateNotificationCommand from a CreateNotificationResource.
    /// </summary>
    /// <param name="resource">The CreateNotificationResource resource</param>
    /// <returns>
    ///     A CreateNotificationCommand assembled from the CreateNotificationResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateNotificationCommand ToCommandFromResource(CreateNotificationResource resource) =>
        new(resource.UserId, resource.AlertId, resource.Message);
}
