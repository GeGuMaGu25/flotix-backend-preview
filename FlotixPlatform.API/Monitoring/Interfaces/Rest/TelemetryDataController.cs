using System.Net.Mime;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Model.Queries;
using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Interfaces.Hubs;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest;

/// <summary>
///     Telemetry records' controller.
/// </summary>
/// <param name="telemetryRecordCommandService">The TelemetryRecord Command Service</param>
/// <param name="telemetryRecordQueryService">The TelemetryRecord Query Service</param>
/// <param name="monitoringHub">The SignalR hub used to push real-time events</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Telemetry Data")]
public class TelemetryDataController(
    ITelemetryRecordCommandService telemetryRecordCommandService,
    ITelemetryRecordQueryService telemetryRecordQueryService,
    IHubContext<MonitoringHub, IMonitoringClient> monitoringHub,
    IStringLocalizer<SharedResource> localizer,
    ILogger<TelemetryDataController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a telemetry record.
    /// </summary>
    /// <param name="resource">CreateTelemetryRecordResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created telemetry record, or an error response if the telemetry record was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(TelemetryRecordResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateTelemetryRecord([FromBody] CreateTelemetryRecordResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createTelemetryRecordCommand = CreateTelemetryRecordCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await telemetryRecordCommandService.Handle(createTelemetryRecordCommand, cancellationToken);
            if (result is Result<TelemetryRecord, CreateTelemetryRecordError>.Success success)
                await monitoringHub.Clients.All.TelemetryRecordReceived(
                    TelemetryRecordResourceFromEntityAssembler.ToResourceFromEntity(success.Value));
            return ActionResultFromCreateTelemetryRecordResultAssembler.ToActionResultFromCreateTelemetryRecordResult(
                result,
                this,
                localizer,
                nameof(GetTelemetryRecordById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating telemetry record");
            return BadRequest(localizer["InvalidTelemetryRecordRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating telemetry record");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingTelemetryRecord"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all telemetry records, optionally filtered by device ID.
    /// </summary>
    /// <param name="deviceId">The device ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching telemetry records (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TelemetryRecordResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllTelemetryRecords([FromQuery] int? deviceId = null,
        CancellationToken cancellationToken = default)
    {
        var telemetryRecords = deviceId is not null
            ? await telemetryRecordQueryService.Handle(new GetAllTelemetryRecordsByDeviceIdQuery(deviceId.Value), cancellationToken)
            : await telemetryRecordQueryService.Handle(new GetAllTelemetryRecordsQuery(), cancellationToken);
        var resources = telemetryRecords.Select(TelemetryRecordResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a telemetry record by id.
    /// </summary>
    /// <param name="id">The TelemetryRecord ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the telemetry record, or not found if the telemetry record was not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TelemetryRecordResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTelemetryRecordById(int id, CancellationToken cancellationToken = default)
    {
        var getTelemetryRecordByIdQuery = new GetTelemetryRecordByIdQuery(id);
        var result = await telemetryRecordQueryService.Handle(getTelemetryRecordByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = TelemetryRecordResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}
