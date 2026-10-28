using System.Net.Mime;
using FlotixPlatform.API.Analytics.Application.Services;
using FlotixPlatform.API.Analytics.Domain.Model.Commands;
using FlotixPlatform.API.Analytics.Domain.Model.Queries;
using FlotixPlatform.API.Analytics.Interfaces.Rest.Resources;
using FlotixPlatform.API.Analytics.Interfaces.Rest.Transform;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Analytics.Interfaces.Rest;

/// <summary>
///     Reports' controller.
/// </summary>
/// <param name="reportCommandService">The Report Command Service</param>
/// <param name="reportQueryService">The Report Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Reports")]
[Authorize(Role.Owner)]
public class ReportsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<ReportsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a report.
    /// </summary>
    /// <param name="resource">CreateReportResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created report, or an error response if the report was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateReport([FromBody] CreateReportResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createReportCommand = CreateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await reportCommandService.Handle(createReportCommand, cancellationToken);
            return ActionResultFromCreateReportResultAssembler.ToActionResultFromCreateReportResult(
                result,
                this,
                localizer,
                nameof(GetReportById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating report");
            return BadRequest(localizer["InvalidReportRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating report");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingReport"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all reports, optionally filtered by owner ID.
    /// </summary>
    /// <param name="ownerId">The owner ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching reports (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReportResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllReports([FromQuery] int? ownerId = null,
        CancellationToken cancellationToken = default)
    {
        var reports = ownerId is not null
            ? await reportQueryService.Handle(new GetAllReportsByOwnerIdQuery(ownerId.Value), cancellationToken)
            : await reportQueryService.Handle(new GetAllReportsQuery(), cancellationToken);
        var resources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a report by id.
    /// </summary>
    /// <param name="id">The Report ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the report, or not found if the report was not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetReportById(int id, CancellationToken cancellationToken = default)
    {
        var getReportByIdQuery = new GetReportByIdQuery(id);
        var result = await reportQueryService.Handle(getReportByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = ReportResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Deletes a report.
    /// </summary>
    /// <param name="id">The Report ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the report was not found.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteReport(int id, CancellationToken cancellationToken)
    {
        var deleteReportCommand = new DeleteReportCommand(id);
        var result = await reportCommandService.Handle(deleteReportCommand, cancellationToken);
        return ActionResultFromDeleteReportResultAssembler.ToActionResultFromDeleteReportResult(
            result,
            this,
            localizer);
    }
}
