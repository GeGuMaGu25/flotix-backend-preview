using System.Net.Mime;
using FlotixPlatform.API.IAM.Application.Services;
using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.Queries;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;
using FlotixPlatform.API.IAM.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.IAM.Interfaces.Rest;

/// <summary>
///     Users' controller: user profiles management.
/// </summary>
/// <param name="userCommandService">The User Command Service</param>
/// <param name="userQueryService">The User Query Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Users")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<UsersController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Gets all users, optionally filtered by role type.
    /// </summary>
    /// <param name="roleType">The role type to filter by (optional)</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the matching users (an empty list if none exist).
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAllUsers([FromQuery] Role? roleType = null,
        CancellationToken cancellationToken = default)
    {
        var users = roleType is not null
            ? await userQueryService.Handle(new GetAllUsersByRoleQuery(roleType.Value), cancellationToken)
            : await userQueryService.Handle(new GetAllUsersQuery(), cancellationToken);
        var resources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    ///     Gets a user by id.
    /// </summary>
    /// <param name="id">The User ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the user, or not found if the user was not found.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetUserById(Guid id, CancellationToken cancellationToken = default)
    {
        var getUserByIdQuery = new GetUserByIdQuery(id);
        var result = await userQueryService.Handle(getUserByIdQuery, cancellationToken);
        if (result is null) return NotFound();
        var resource = UserResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Updates the profile of a user.
    /// </summary>
    /// <param name="id">The User ID</param>
    /// <param name="resource">UpdateUserProfileResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the updated user, or an error response if the user was not updated.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateUserProfile(Guid id, [FromBody] UpdateUserProfileResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var updateUserProfileCommand = UpdateUserProfileCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await userCommandService.Handle(updateUserProfileCommand, cancellationToken);
            return ActionResultFromUpdateUserProfileResultAssembler.ToActionResultFromUpdateUserProfileResult(
                result,
                this,
                localizer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while updating user {UserId}", id);

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorUpdatingUser"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Deletes a user.
    /// </summary>
    /// <param name="id">The User ID</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result with no content, or not found if the user was not found.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var deleteUserCommand = new DeleteUserCommand(id);
        var result = await userCommandService.Handle(deleteUserCommand, cancellationToken);
        return ActionResultFromDeleteUserResultAssembler.ToActionResultFromDeleteUserResult(
            result,
            this,
            localizer);
    }
}
