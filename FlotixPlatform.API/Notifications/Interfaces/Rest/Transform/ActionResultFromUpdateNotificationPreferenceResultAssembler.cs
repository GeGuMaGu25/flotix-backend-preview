using FlotixPlatform.API.Notifications.Application.Errors;
using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update a notification preference in the application layer.
/// </summary>
public static class ActionResultFromUpdateNotificationPreferenceResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update notification preference operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update a notification preference.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateNotificationPreferenceResult(
        Result<NotificationPreference, UpdateNotificationPreferenceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<NotificationPreference, UpdateNotificationPreferenceError>.Success success =>
                controller.Ok(NotificationPreferenceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<NotificationPreference, UpdateNotificationPreferenceError>.Failure failure =>
                failure.Error switch
                {
                    UpdateNotificationPreferenceError.NotificationPreferenceNotFound =>
                        controller.NotFound(localizer["NotificationPreferenceNotFound"].Value),
                    UpdateNotificationPreferenceError.InvalidNotificationPreference =>
                        controller.BadRequest(localizer["InvalidNotificationPreferenceRequest"].Value),
                    UpdateNotificationPreferenceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingNotificationPreference"].Value,
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
