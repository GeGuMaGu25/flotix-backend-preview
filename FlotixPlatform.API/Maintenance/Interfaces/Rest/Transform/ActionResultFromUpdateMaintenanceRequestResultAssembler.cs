using FlotixPlatform.API.Maintenance.Application.Errors;
using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update a maintenance request in the application layer.
/// </summary>
public static class ActionResultFromUpdateMaintenanceRequestResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update maintenance request operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update a maintenance request.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateMaintenanceRequestResult(
        Result<MaintenanceRequest, UpdateMaintenanceRequestError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Success success =>
                controller.Ok(MaintenanceRequestResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<MaintenanceRequest, UpdateMaintenanceRequestError>.Failure failure =>
                failure.Error switch
                {
                    UpdateMaintenanceRequestError.MaintenanceRequestNotFound =>
                        controller.NotFound(localizer["MaintenanceRequestNotFound"].Value),
                    UpdateMaintenanceRequestError.InvalidMaintenanceRequest =>
                        controller.BadRequest(localizer["InvalidMaintenanceRequestRequest"].Value),
                    UpdateMaintenanceRequestError.MaintenanceRequestAlreadyClosed =>
                        controller.Conflict(localizer["MaintenanceRequestAlreadyClosed"].Value),
                    UpdateMaintenanceRequestError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingMaintenanceRequest"].Value,
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
