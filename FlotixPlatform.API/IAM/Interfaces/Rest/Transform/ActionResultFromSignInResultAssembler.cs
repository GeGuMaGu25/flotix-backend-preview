using FlotixPlatform.API.IAM.Application.Errors;
using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to sign in a user in the application layer.
/// </summary>
public static class ActionResultFromSignInResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a sign-in operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to sign in a user.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromSignInResult(
        Result<(User User, string Token), SignInError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<(User User, string Token), SignInError>.Success success =>
                controller.Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(success.Value.User, success.Value.Token)),

            Result<(User User, string Token), SignInError>.Failure failure =>
                failure.Error switch
                {
                    SignInError.InvalidCredentials =>
                        controller.Unauthorized(localizer["InvalidCredentials"].Value),

                    SignInError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorSigningIn"].Value,
                            statusCode: 500),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500)
        };
}
