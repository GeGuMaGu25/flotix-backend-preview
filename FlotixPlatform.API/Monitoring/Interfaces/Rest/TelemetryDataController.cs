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
/// <param name="telemetryDataCommandService">The TelemetryData Command Service</param>
/// <param name="telemetryDataQueryService">The TelemetryData Query Service</param>
/// <param name="monitoringHub">The SignalR hub used to push real-time events</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Telemetry Data")]
public class TelemetryDataController(
    ITelemetryDataCommandService telemetryDataCommandService,
    ITelemetryDataQueryService telemetryDataQueryService,
    IHubContext<MonitoringHub, IMonitoringClient> monitoringHub,
    IStringLocalizer<SharedResource> localizer,
    ILogger<TelemetryDataController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a telemetry record.
    /// </summary>
    /// <param name="resource">CreateTelemetryDataResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created telemetry record, or an error response if the telemetry record was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(TelemetryDataResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateTelemetryData([FromBody] CreateTelemetryDataResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createTelemetryDataCommand = CreateTelemetryDataCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await telemetryDataCommandService.Handle(createTelemetryDataCommand, cancellationToken);
            if (result is Result<TelemetryData, CreateTelemetryDataError>.Success success)
                await monitoringHub.Clients.All.TelemetryDataReceived(
                    TelemetryDataResourceFromEntityAssembler.ToResourceFromEntity(success.Value));
            return ActionResultFromCreateTelemetryDataResultAssembler.ToActionResultFromCreateTelemetryDataResult(
                result,
                this,
                localizer,
                nameof(GetTelemetryDataById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating telemetry record");
            return BadRequest(localizer["InvalidTelemetryDataRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating telemetry record");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingTelemetryData"].Value,
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
    [ProducesResponseType(typeof(IEnumerable<TelemetryDataResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllTelemetryData([FromQuery] Guid? deviceId = null,
        CancellationToken cancellationToken = default)
    {
        var telemetryData = deviceId is not null
            ? await telemetryDataQueryService.Handle(new GetAllTelemetryDataByDeviceIdQuery(deviceId.Value), cancellationToken)
            : await telemetryDataQueryService.Handle(new GetAllTelemetryDataQuery(), cancellationToken);
        var resources = telemetryData.Select(TelemetryDataResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a telemetry record by id.
    /// </summary>
    /// <param name="id">The TelemetryData ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the telemetry record, or not found if the telemetry record was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TelemetryDataResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTelemetryDataById(Guid id, CancellationToken cancellationToken = default)
    {
        var getTelemetryDataByIdQuery = new GetTelemetryDataByIdQuery(id);
        var result = await telemetryDataQueryService.Handle(getTelemetryDataByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = TelemetryDataResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
}
