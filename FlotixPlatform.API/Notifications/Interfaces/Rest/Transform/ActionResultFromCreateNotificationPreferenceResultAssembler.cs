using FlotixPlatform.API.Notifications.Application.Errors;
using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a notification preference in the application layer.
/// </summary>
public static class ActionResultFromCreateNotificationPreferenceResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create notification preference operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a notification preference.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getNotificationPreferenceByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateNotificationPreferenceResult(
        Result<NotificationPreference, CreateNotificationPreferenceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getNotificationPreferenceByIdActionName) =>
        result switch
        {
            Result<NotificationPreference, CreateNotificationPreferenceError>.Success success =>
                controller.CreatedAtAction(getNotificationPreferenceByIdActionName, new { id = success.Value.Id },
                    NotificationPreferenceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<NotificationPreference, CreateNotificationPreferenceError>.Failure failure =>
                failure.Error switch
                {
                    CreateNotificationPreferenceError.InvalidNotificationPreference =>
                        controller.BadRequest(localizer["InvalidNotificationPreferenceRequest"].Value),
                    CreateNotificationPreferenceError.DuplicateNotificationPreferenceUserId =>
                        controller.Conflict(localizer["DuplicateNotificationPreferenceUserId"].Value),
                    CreateNotificationPreferenceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingNotificationPreference"].Value,
                            statusCode: 500),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500)
        };
}
