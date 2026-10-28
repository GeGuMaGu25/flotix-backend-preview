using System.Net.Mime;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Domain.Model.Queries;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Resources;
using FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest;

/// <summary>
///     IoT devices' controller.
/// </summary>
/// <param name="iotDeviceCommandService">The IotDevice Command Service</param>
/// <param name="iotDeviceQueryService">The IotDevice Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("IoT devices")]
public class IotDevicesController(
    IIotDeviceCommandService iotDeviceCommandService,
    IIotDeviceQueryService iotDeviceQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<IotDevicesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates an IoT device.
    /// </summary>
    /// <param name="resource">CreateIotDeviceResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created IoT device, or an error response if the IoT device was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateIotDevice([FromBody] CreateIotDeviceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createIotDeviceCommand = CreateIotDeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await iotDeviceCommandService.Handle(createIotDeviceCommand, cancellationToken);
            return ActionResultFromCreateIotDeviceResultAssembler.ToActionResultFromCreateIotDeviceResult(
                result,
                this,
                localizer,
                nameof(GetIotDeviceById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating IoT device");
            return BadRequest(localizer["InvalidIotDeviceRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating IoT device");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingIotDevice"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all IoT devices, optionally filtered by vehicle ID.
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching IoT devices (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IotDeviceResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllIotDevices([FromQuery] Guid? vehicleId = null,
        CancellationToken cancellationToken = default)
    {
        var iotDevices = vehicleId is not null
            ? await iotDeviceQueryService.Handle(new GetAllIotDevicesByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : await iotDeviceQueryService.Handle(new GetAllIotDevicesQuery(), cancellationToken);
        var resources = iotDevices.Select(IotDeviceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets an IoT device by id.
    /// </summary>
    /// <param name="id">The IotDevice ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the IoT device, or not found if the IoT device was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIotDeviceById(Guid id, CancellationToken cancellationToken = default)
    {
        var getIotDeviceByIdQuery = new GetIotDeviceByIdQuery(id);
        var result = await iotDeviceQueryService.Handle(getIotDeviceByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = IotDeviceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates an IoT device.
    /// </summary>
    /// <param name="id">The IotDevice ID</param>
    /// <param name="resource">UpdateIotDeviceResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated IoT device, or an error response if the IoT device was
    ///     not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(IotDeviceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateIotDevice(Guid id, [FromBody] UpdateIotDeviceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateIotDeviceCommand = UpdateIotDeviceCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await iotDeviceCommandService.Handle(updateIotDeviceCommand, cancellationToken);
            return ActionResultFromUpdateIotDeviceResultAssembler.ToActionResultFromUpdateIotDeviceResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating IoT device {IotDeviceId}", id);
            return BadRequest(localizer["InvalidIotDeviceRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating IoT device {IotDeviceId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingIotDevice"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes an IoT device.
    /// </summary>
    /// <param name="id">The IotDevice ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the IoT device was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteIotDevice(Guid id, CancellationToken cancellationToken)
    {
        var deleteIotDeviceCommand = new DeleteIotDeviceCommand(id);
        var result = await iotDeviceCommandService.Handle(deleteIotDeviceCommand, cancellationToken);
        return ActionResultFromDeleteIotDeviceResultAssembler.ToActionResultFromDeleteIotDeviceResult(
            result,
            this,
            localizer);
    }
}
