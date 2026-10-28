using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a telemetry record in the application layer.
/// </summary>
public static class ActionResultFromCreateTelemetryDataResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create telemetry record operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a telemetry record.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getTelemetryDataByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateTelemetryDataResult(
        Result<TelemetryData, CreateTelemetryDataError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getTelemetryDataByIdActionName) =>
        result switch
        {
            Result<TelemetryData, CreateTelemetryDataError>.Success success =>
                controller.CreatedAtAction(getTelemetryDataByIdActionName, new { id = success.Value.Id },
                    TelemetryDataResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<TelemetryData, CreateTelemetryDataError>.Failure failure =>
                failure.Error switch
                {
                    CreateTelemetryDataError.InvalidTelemetryData =>
                        controller.BadRequest(localizer["InvalidTelemetryDataRequest"].Value),
                    CreateTelemetryDataError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["TelemetryDataReferencedResourceNotFound"].Value),
                    CreateTelemetryDataError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingTelemetryData"].Value,
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
