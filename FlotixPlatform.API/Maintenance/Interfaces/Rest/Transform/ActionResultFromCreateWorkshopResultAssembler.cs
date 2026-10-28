using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a workshop in the application layer.
/// </summary>
public static class ActionResultFromCreateWorkshopResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create workshop operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a workshop.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getWorkshopByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateWorkshopResult(
        Result<Workshop, CreateWorkshopError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getWorkshopByIdActionName) =>
        result switch
        {
            Result<Workshop, CreateWorkshopError>.Success success =>
                controller.CreatedAtAction(getWorkshopByIdActionName, new { id = success.Value.Id },
                    WorkshopResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Workshop, CreateWorkshopError>.Failure failure =>
                failure.Error switch
                {
                    CreateWorkshopError.InvalidWorkshop =>
                        controller.BadRequest(localizer["InvalidWorkshopRequest"].Value),
                    CreateWorkshopError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingWorkshop"].Value,
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
