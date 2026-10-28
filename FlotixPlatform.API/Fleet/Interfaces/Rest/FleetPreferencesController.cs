using System.Net.Mime;
using FlotixPlatform.API.Fleet.Application.Services;
using FlotixPlatform.API.Fleet.Domain.Model.Queries;
using FlotixPlatform.API.Fleet.Interfaces.Rest.Resources;
using FlotixPlatform.API.Fleet.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Fleet.Interfaces.Rest;

/// <summary>
///     Fleet preferences' controller.
/// </summary>
/// <remarks>
///     Singular resource: the fleet has a single set of preferences.
/// </remarks>
/// <param name="fleetPreferencesCommandService">The Fleet Preferences Command Service</param>
/// <param name="fleetPreferencesQueryService">The Fleet Preferences Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Fleet Preferences")]
public class FleetPreferencesController(
    IFleetPreferencesCommandService fleetPreferencesCommandService,
    IFleetPreferencesQueryService fleetPreferencesQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<FleetPreferencesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Gets the fleet preferences.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the fleet preferences, or not found if they were not seeded.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(FleetPreferencesResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetFleetPreferences(CancellationToken cancellationToken = default)
    {
        var getFleetPreferencesQuery = new GetFleetPreferencesQuery();
        var result = await fleetPreferencesQueryService.Handle(getFleetPreferencesQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = FleetPreferencesResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates the fleet preferences.
    /// </summary>
    /// <param name="resource">UpdateFleetPreferencesResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated fleet preferences, or an error response if they were not
    ///     updated.
    /// </returns>
    [HttpPut]
    [ProducesResponseType(typeof(FleetPreferencesResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateFleetPreferences([FromBody] UpdateFleetPreferencesResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateFleetPreferencesCommand = UpdateFleetPreferencesCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await fleetPreferencesCommandService.Handle(updateFleetPreferencesCommand, cancellationToken);
            return ActionResultFromUpdateFleetPreferencesResultAssembler.ToActionResultFromUpdateFleetPreferencesResult(
                result,
                this,
                localizer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating fleet preferences");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingFleetPreferences"].Value,
                statusCode: 500);
        }
    }
}
