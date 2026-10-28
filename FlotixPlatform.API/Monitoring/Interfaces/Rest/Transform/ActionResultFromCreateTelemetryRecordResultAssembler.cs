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
public static class ActionResultFromCreateTelemetryRecordResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create telemetry record operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a telemetry record.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getTelemetryRecordByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateTelemetryRecordResult(
        Result<TelemetryRecord, CreateTelemetryRecordError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getTelemetryRecordByIdActionName) =>
        result switch
        {
            Result<TelemetryRecord, CreateTelemetryRecordError>.Success success =>
                controller.CreatedAtAction(getTelemetryRecordByIdActionName, new { id = success.Value.Id },
                    TelemetryRecordResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<TelemetryRecord, CreateTelemetryRecordError>.Failure failure =>
                failure.Error switch
                {
                    CreateTelemetryRecordError.InvalidTelemetryRecord =>
                        controller.BadRequest(localizer["InvalidTelemetryRecordRequest"].Value),
                    CreateTelemetryRecordError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingTelemetryRecord"].Value,
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
