using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to receive a telemetry packet in the application layer.
/// </summary>
public static class ActionResultFromReceiveTelemetryDataResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a receive telemetry data operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to receive a telemetry packet.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromReceiveTelemetryDataResult(
        Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Success =>
                controller.Accepted(),

            Result<ReceiveTelemetryDataCommand, ReceiveTelemetryDataError>.Failure failure =>
                failure.Error switch
                {
                    ReceiveTelemetryDataError.InvalidTelemetryData =>
                        controller.BadRequest(localizer["InvalidTelemetryDataRequest"].Value),

                    ReceiveTelemetryDataError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorReceivingTelemetryData"].Value,
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
