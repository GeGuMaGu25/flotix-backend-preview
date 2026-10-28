using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update a workshop in the application layer.
/// </summary>
public static class ActionResultFromUpdateWorkshopResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update workshop operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update a workshop.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateWorkshopResult(
        Result<Workshop, UpdateWorkshopError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<Workshop, UpdateWorkshopError>.Success success =>
                controller.Ok(WorkshopResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Workshop, UpdateWorkshopError>.Failure failure =>
                failure.Error switch
                {
                    UpdateWorkshopError.WorkshopNotFound =>
                        controller.NotFound(localizer["WorkshopNotFound"].Value),
                    UpdateWorkshopError.InvalidWorkshop =>
                        controller.BadRequest(localizer["InvalidWorkshopRequest"].Value),
                    UpdateWorkshopError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingWorkshop"].Value,
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
