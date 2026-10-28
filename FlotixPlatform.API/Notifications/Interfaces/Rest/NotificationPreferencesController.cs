using System.Net.Mime;
using FlotixPlatform.API.Notifications.Application.Services;
using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Domain.Model.Queries;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;
using FlotixPlatform.API.Notifications.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest;

/// <summary>
///     Notification preferences' controller.
/// </summary>
/// <param name="notificationPreferenceCommandService">The NotificationPreference Command Service</param>
/// <param name="notificationPreferenceQueryService">The NotificationPreference Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Notification Preferences")]
public class NotificationPreferencesController(
    INotificationPreferenceCommandService notificationPreferenceCommandService,
    INotificationPreferenceQueryService notificationPreferenceQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<NotificationPreferencesController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Creates a notification preference.
    /// </summary>
    /// <param name="resource">CreateNotificationPreferenceResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created notification preference, or an error response if the notification preference was
    ///     not created.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(NotificationPreferenceResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> CreateNotificationPreference([FromBody] CreateNotificationPreferenceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var createNotificationPreferenceCommand = CreateNotificationPreferenceCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await notificationPreferenceCommandService.Handle(createNotificationPreferenceCommand, cancellationToken);
            return ActionResultFromCreateNotificationPreferenceResultAssembler.ToActionResultFromCreateNotificationPreferenceResult(
                result,
                this,
                localizer,
                nameof(GetNotificationPreferenceById));
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while creating notification preference");
            return BadRequest(localizer["InvalidNotificationPreferenceRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while creating notification preference");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorCreatingNotificationPreference"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Gets all notification preferences, optionally filtered by user ID.
    /// </summary>
    /// <param name="userId">The user ID to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching notification preferences (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<NotificationPreferenceResource>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllNotificationPreferences([FromQuery] int? userId = null,
        CancellationToken cancellationToken = default)
    {
        var notificationPreferences = userId is not null
            ? await notificationPreferenceQueryService.Handle(new GetAllNotificationPreferencesByUserIdQuery(userId.Value), cancellationToken)
            : await notificationPreferenceQueryService.Handle(new GetAllNotificationPreferencesQuery(), cancellationToken);
        var resources = notificationPreferences.Select(NotificationPreferenceResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a notification preference by id.
    /// </summary>
    /// <param name="id">The NotificationPreference ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the notification preference, or not found if the notification preference was not found.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(NotificationPreferenceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetNotificationPreferenceById(int id, CancellationToken cancellationToken = default)
    {
        var getNotificationPreferenceByIdQuery = new GetNotificationPreferenceByIdQuery(id);
        var result = await notificationPreferenceQueryService.Handle(getNotificationPreferenceByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = NotificationPreferenceResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates a notification preference.
    /// </summary>
    /// <param name="id">The NotificationPreference ID</param>
    /// <param name="resource">UpdateNotificationPreferenceResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated notification preference, or an error response if the notification preference was
    ///     not updated.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(NotificationPreferenceResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateNotificationPreference(int id, [FromBody] UpdateNotificationPreferenceResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateNotificationPreferenceCommand = UpdateNotificationPreferenceCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await notificationPreferenceCommandService.Handle(updateNotificationPreferenceCommand, cancellationToken);
            return ActionResultFromUpdateNotificationPreferenceResultAssembler.ToActionResultFromUpdateNotificationPreferenceResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while updating notification preference {NotificationPreferenceId}", id);
            return BadRequest(localizer["InvalidNotificationPreferenceRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating notification preference {NotificationPreferenceId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingNotificationPreference"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a notification preference.
    /// </summary>
    /// <param name="id">The NotificationPreference ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the notification preference was not found.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteNotificationPreference(int id, CancellationToken cancellationToken)
    {
        var deleteNotificationPreferenceCommand = new DeleteNotificationPreferenceCommand(id);
        var result = await notificationPreferenceCommandService.Handle(deleteNotificationPreferenceCommand, cancellationToken);
        return ActionResultFromDeleteNotificationPreferenceResultAssembler.ToActionResultFromDeleteNotificationPreferenceResult(
            result,
            this,
            localizer);
    }
}
