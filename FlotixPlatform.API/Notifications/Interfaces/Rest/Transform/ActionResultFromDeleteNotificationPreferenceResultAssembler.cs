using FlotixPlatform.API.Notifications.Application.Errors;
using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to delete a notification preference in the application layer.
/// </summary>
public static class ActionResultFromDeleteNotificationPreferenceResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a delete notification preference operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to delete a notification preference.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromDeleteNotificationPreferenceResult(
        Result<NotificationPreference, DeleteNotificationPreferenceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<NotificationPreference, DeleteNotificationPreferenceError>.Success success =>
                controller.NoContent(),

            Result<NotificationPreference, DeleteNotificationPreferenceError>.Failure failure =>
                failure.Error switch
                {
                    DeleteNotificationPreferenceError.NotificationPreferenceNotFound =>
                        controller.NotFound(localizer["NotificationPreferenceNotFound"].Value),
                    DeleteNotificationPreferenceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingNotificationPreference"].Value,
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
