using System.Net.Mime;
using FlotixPlatform.API.Maintenance.Application.Services;
using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.Queries;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest;

/// <summary>
///     Maintenance requests' controller.
/// </summary>
/// <param name="maintenanceRequestCommandService">The MaintenanceRequest Command Service</param>
/// <param name="maintenanceRequestQueryService">The MaintenanceRequest Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Maintenance Requests")]
public class MaintenanceRequestsController(
    IMaintenanceRequestCommandService maintenanceRequestCommandService,
    IMaintenanceRequestQueryService maintenanceRequestQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<MaintenanceRequestsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a maintenance request.
    /// </summary>
    /// <param name="resource">CreateMaintenanceRequestResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created maintenance request, or an error response if the maintenance request was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateMaintenanceRequest([FromBody] CreateMaintenanceRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createMaintenanceRequestCommand = CreateMaintenanceRequestCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await maintenanceRequestCommandService.Handle(createMaintenanceRequestCommand, cancellationToken);
            return ActionResultFromCreateMaintenanceRequestResultAssembler.ToActionResultFromCreateMaintenanceRequestResult(
                result,
                this,
                localizer,
                nameof(GetMaintenanceRequestById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating maintenance request");
            return BadRequest(localizer["InvalidMaintenanceRequestRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating maintenance request");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingMaintenanceRequest"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all maintenance requests, optionally filtered by vehicle ID or mechanic ID.
    /// </summary>
    /// <param name="vehicleId">The vehicle ID to filter by (optional)</param>
    /// <param name="mechanicId">The mechanic ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching maintenance requests (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceRequestResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllMaintenanceRequests([FromQuery] int? vehicleId = null,
        [FromQuery] int? mechanicId = null,
        CancellationToken cancellationToken = default)
    {
        var maintenanceRequests = vehicleId is not null
            ? await maintenanceRequestQueryService.Handle(new GetAllMaintenanceRequestsByVehicleIdQuery(vehicleId.Value), cancellationToken)
            : mechanicId is not null
            ? await maintenanceRequestQueryService.Handle(new GetAllMaintenanceRequestsByMechanicIdQuery(mechanicId.Value), cancellationToken)
            : await maintenanceRequestQueryService.Handle(new GetAllMaintenanceRequestsQuery(), cancellationToken);
        var resources = maintenanceRequests.Select(MaintenanceRequestResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a maintenance request by id.
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the maintenance request, or not found if the maintenance request was not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetMaintenanceRequestById(int id, CancellationToken cancellationToken = default)
    {
        var getMaintenanceRequestByIdQuery = new GetMaintenanceRequestByIdQuery(id);
        var result = await maintenanceRequestQueryService.Handle(getMaintenanceRequestByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = MaintenanceRequestResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates a maintenance request.
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="resource">UpdateMaintenanceRequestResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated maintenance request, or an error response if the maintenance request was
    ///     not updated.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MaintenanceRequestResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateMaintenanceRequest(int id, [FromBody] UpdateMaintenanceRequestResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateMaintenanceRequestCommand = UpdateMaintenanceRequestCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await maintenanceRequestCommandService.Handle(updateMaintenanceRequestCommand, cancellationToken);
            return ActionResultFromUpdateMaintenanceRequestResultAssembler.ToActionResultFromUpdateMaintenanceRequestResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating maintenance request {MaintenanceRequestId}", id);
            return BadRequest(localizer["InvalidMaintenanceRequestRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating maintenance request {MaintenanceRequestId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingMaintenanceRequest"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a maintenance request.
    /// </summary>
    /// <param name="id">The MaintenanceRequest ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the maintenance request was not found.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteMaintenanceRequest(int id, CancellationToken cancellationToken)
    {
        var deleteMaintenanceRequestCommand = new DeleteMaintenanceRequestCommand(id);
        var result = await maintenanceRequestCommandService.Handle(deleteMaintenanceRequestCommand, cancellationToken);
        return ActionResultFromDeleteMaintenanceRequestResultAssembler.ToActionResultFromDeleteMaintenanceRequestResult(
            result,
            this,
            localizer);
    }
}
