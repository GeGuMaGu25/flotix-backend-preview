using FlotixPlatform.API.Analytics.Application.Errors;
using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create a report in the application layer.
/// </summary>
public static class ActionResultFromCreateReportResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create report operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create a report.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getReportByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateReportResult(
        Result<Report, CreateReportError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getReportByIdActionName) =>
        result switch
        {
            Result<Report, CreateReportError>.Success success =>
                controller.CreatedAtAction(getReportByIdActionName, new { id = success.Value.Id },
                    ReportResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<Report, CreateReportError>.Failure failure =>
                failure.Error switch
                {
                    CreateReportError.InvalidReport =>
                        controller.BadRequest(localizer["InvalidReportRequest"].Value),
                    CreateReportError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["ReportReferencedResourceNotFound"].Value),
                    CreateReportError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingReport"].Value,
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
