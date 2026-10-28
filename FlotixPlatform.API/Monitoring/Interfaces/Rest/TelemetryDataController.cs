using System.Net.Mime;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Model.Queries;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest;

/// <summary>
///     Telemetry data controller: ingestion and query of the telemetry sent by IoT devices.
/// </summary>
/// <param name="telemetryDataCommandService">The TelemetryData Command Service</param>
/// <param name="telemetryDataQueryService">The TelemetryData Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Telemetry Data")]
public class TelemetryDataController(
    ITelemetryDataCommandService telemetryDataCommandService,
    ITelemetryDataQueryService telemetryDataQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<TelemetryDataController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Receives a telemetry packet sent by an IoT device.
    /// </summary>
    /// <remarks>
    ///     Valid packets are queued and processed in the background by the IoT Telemetry Processor; corrupt packets
    ///     are discarded and recorded in the error log.
    /// </remarks>
    /// <param name="resource">ReceiveTelemetryDataResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An accepted response when the packet is queued, or bad request if the packet is corrupt.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ReceiveTelemetryData([FromBody] ReceiveTelemetryDataResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var receiveTelemetryDataCommand = ReceiveTelemetryDataCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await telemetryDataCommandService.Handle(receiveTelemetryDataCommand, cancellationToken);
            return ActionResultFromReceiveTelemetryDataResultAssembler.ToActionResultFromReceiveTelemetryDataResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Corrupt telemetry packet discarded");
            return BadRequest(localizer["InvalidTelemetryDataRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while receiving telemetry packet");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorReceivingTelemetryData"].Value,
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
