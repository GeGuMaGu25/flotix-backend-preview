using System.Net.Mime;
using FlotixPlatform.API.IAM.Application.Services;
using FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;
using FlotixPlatform.API.IAM.Interfaces.Rest.Transform;
using FlotixPlatform.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.IAM.Interfaces.Rest;

/// <summary>
///     Authentication controller: public sign-up and sign-in endpoints.
/// </summary>
/// <param name="userCommandService">The User Command Service</param>
/// <param name="localizer">String localizer for response messages</param>
/// <param name="logger">Logger for diagnostics and error reporting</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Authentication")]
[AllowAnonymous]
public class AuthenticationController(
    IUserCommandService userCommandService,
    IStringLocalizer<SharedResource> localizer,
    ILogger<AuthenticationController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Signs up a user.
    /// </summary>
    /// <param name="resource">SignUpResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the created user, or an error response if the user was not created.
    /// </returns>
    [HttpPost("sign-up")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SignUp([FromBody] SignUpResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await userCommandService.Handle(signUpCommand, cancellationToken);
            return ActionResultFromSignUpResultAssembler.ToActionResultFromSignUpResult(
                result,
                this,
                localizer,
                nameof(UsersController.GetUserById),
                "Users");
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while signing up user");
            return BadRequest(localizer["InvalidSignUpRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while signing up user");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorSigningUp"].Value,
                statusCode: 500);
        }
    }

    /// <summary>
    ///     Signs in a user.
    /// </summary>
    /// <param name="resource">SignInResource resource</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A response as an action result containing the authenticated user and its bearer token, or unauthorized if the
    ///     credentials are not valid.
    /// </returns>
    [HttpPost("sign-in")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SignIn([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        try
        {
            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await userCommandService.Handle(signInCommand, cancellationToken);
            return ActionResultFromSignInResultAssembler.ToActionResultFromSignInResult(
                result,
                this,
                localizer);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation failed while signing in user");
            return BadRequest(localizer["InvalidSignInRequest"].Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error while signing in user");

            return Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorSigningIn"].Value,
                statusCode: 500);
        }
    }
}
