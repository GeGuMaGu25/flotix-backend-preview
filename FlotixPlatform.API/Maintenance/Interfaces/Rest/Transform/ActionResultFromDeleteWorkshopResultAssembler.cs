using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to delete a workshop in the application layer.
/// </summary>
public static class ActionResultFromDeleteWorkshopResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a delete workshop operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to delete a workshop.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromDeleteWorkshopResult(
        Result<Workshop, DeleteWorkshopError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<Workshop, DeleteWorkshopError>.Success success =>
                controller.NoContent(),

            Result<Workshop, DeleteWorkshopError>.Failure failure =>
                failure.Error switch
                {
                    DeleteWorkshopError.WorkshopNotFound =>
                        controller.NotFound(localizer["WorkshopNotFound"].Value),
                    DeleteWorkshopError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingWorkshop"].Value,
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
