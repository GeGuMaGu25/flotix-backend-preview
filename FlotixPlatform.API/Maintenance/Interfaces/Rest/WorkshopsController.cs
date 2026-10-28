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
///     Workshops' controller.
/// </summary>
/// <param name="workshopCommandService">The Workshop Command Service</param>
/// <param name="workshopQueryService">The Workshop Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Workshops")]
public class WorkshopsController(
    IWorkshopCommandService workshopCommandService,
    IWorkshopQueryService workshopQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<WorkshopsController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a workshop.
    /// </summary>
    /// <param name="resource">CreateWorkshopResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created workshop, or an error response if the workshop was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(WorkshopResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateWorkshop([FromBody] CreateWorkshopResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createWorkshopCommand = CreateWorkshopCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await workshopCommandService.Handle(createWorkshopCommand, cancellationToken);
            return ActionResultFromCreateWorkshopResultAssembler.ToActionResultFromCreateWorkshopResult(
                result,
                this,
                localizer,
                nameof(GetWorkshopById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating workshop");
            return BadRequest(localizer["InvalidWorkshopRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating workshop");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingWorkshop"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all workshops, optionally filtered by mechanic ID.
    /// </summary>
    /// <param name="mechanicId">The mechanic ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching workshops (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WorkshopResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllWorkshops([FromQuery] int? mechanicId = null,
        CancellationToken cancellationToken = default)
    {
        var workshops = mechanicId is not null
            ? await workshopQueryService.Handle(new GetAllWorkshopsByMechanicIdQuery(mechanicId.Value), cancellationToken)
            : await workshopQueryService.Handle(new GetAllWorkshopsQuery(), cancellationToken);
        var resources = workshops.Select(WorkshopResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a workshop by id.
    /// </summary>
    /// <param name="id">The Workshop ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the workshop, or not found if the workshop was not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WorkshopResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetWorkshopById(int id, CancellationToken cancellationToken = default)
    {
        var getWorkshopByIdQuery = new GetWorkshopByIdQuery(id);
        var result = await workshopQueryService.Handle(getWorkshopByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = WorkshopResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates a workshop.
    /// </summary>
    /// <param name="id">The Workshop ID</param>
    /// <param name="resource">UpdateWorkshopResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated workshop, or an error response if the workshop was
    ///     not updated.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(WorkshopResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateWorkshop(int id, [FromBody] UpdateWorkshopResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateWorkshopCommand = UpdateWorkshopCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await workshopCommandService.Handle(updateWorkshopCommand, cancellationToken);
            return ActionResultFromUpdateWorkshopResultAssembler.ToActionResultFromUpdateWorkshopResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating workshop {WorkshopId}", id);
            return BadRequest(localizer["InvalidWorkshopRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating workshop {WorkshopId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingWorkshop"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a workshop.
    /// </summary>
    /// <param name="id">The Workshop ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the workshop was not found.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteWorkshop(int id, CancellationToken cancellationToken)
    {
        var deleteWorkshopCommand = new DeleteWorkshopCommand(id);
        var result = await workshopCommandService.Handle(deleteWorkshopCommand, cancellationToken);
        return ActionResultFromDeleteWorkshopResultAssembler.ToActionResultFromDeleteWorkshopResult(
            result,
            this,
            localizer);
    }
}
