using System.Net.Mime;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Domain.Model.Queries;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Resources;
using FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest;

/// <summary>
///     IoT catalog items' controller.
/// </summary>
/// <param name="iotCatalogItemCommandService">The IotCatalogItem Command Service</param>
/// <param name="iotCatalogItemQueryService">The IotCatalogItem Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("IoT Catalog")]
public class IotCatalogController(
    IIotCatalogItemCommandService iotCatalogItemCommandService,
    IIotCatalogItemQueryService iotCatalogItemQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<IotCatalogController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates an IoT catalog item.
    /// </summary>
    /// <param name="resource">CreateIotCatalogItemResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created IoT catalog item, or an error response if the IoT catalog item was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(IotCatalogItemResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateIotCatalogItem([FromBody] CreateIotCatalogItemResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createIotCatalogItemCommand = CreateIotCatalogItemCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await iotCatalogItemCommandService.Handle(createIotCatalogItemCommand, cancellationToken);
            return ActionResultFromCreateIotCatalogItemResultAssembler.ToActionResultFromCreateIotCatalogItemResult(
                result,
                this,
                localizer,
                nameof(GetIotCatalogItemById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating IoT catalog item");
            return BadRequest(localizer["InvalidIotCatalogItemRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating IoT catalog item");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingIotCatalogItem"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all IoT catalog items.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching IoT catalog items (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IotCatalogItemResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllIotCatalogItems(CancellationToken cancellationToken = default)
    {
        var iotCatalogItems = await iotCatalogItemQueryService.Handle(new GetAllIotCatalogItemsQuery(), cancellationToken);
        var resources = iotCatalogItems.Select(IotCatalogItemResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets an IoT catalog item by id.
    /// </summary>
    /// <param name="id">The IotCatalogItem ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the IoT catalog item, or not found if the IoT catalog item was not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IotCatalogItemResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetIotCatalogItemById(int id, CancellationToken cancellationToken = default)
    {
        var getIotCatalogItemByIdQuery = new GetIotCatalogItemByIdQuery(id);
        var result = await iotCatalogItemQueryService.Handle(getIotCatalogItemByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = IotCatalogItemResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates an IoT catalog item.
    /// </summary>
    /// <param name="id">The IotCatalogItem ID</param>
    /// <param name="resource">UpdateIotCatalogItemResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated IoT catalog item, or an error response if the IoT catalog item was
    ///     not updated.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IotCatalogItemResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateIotCatalogItem(int id, [FromBody] UpdateIotCatalogItemResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateIotCatalogItemCommand = UpdateIotCatalogItemCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await iotCatalogItemCommandService.Handle(updateIotCatalogItemCommand, cancellationToken);
            return ActionResultFromUpdateIotCatalogItemResultAssembler.ToActionResultFromUpdateIotCatalogItemResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating IoT catalog item {IotCatalogItemId}", id);
            return BadRequest(localizer["InvalidIotCatalogItemRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating IoT catalog item {IotCatalogItemId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingIotCatalogItem"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes an IoT catalog item.
    /// </summary>
    /// <param name="id">The IotCatalogItem ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the IoT catalog item was not found.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteIotCatalogItem(int id, CancellationToken cancellationToken)
    {
        var deleteIotCatalogItemCommand = new DeleteIotCatalogItemCommand(id);
        var result = await iotCatalogItemCommandService.Handle(deleteIotCatalogItemCommand, cancellationToken);
        return ActionResultFromDeleteIotCatalogItemResultAssembler.ToActionResultFromDeleteIotCatalogItemResult(
            result,
            this,
            localizer);
    }
}
