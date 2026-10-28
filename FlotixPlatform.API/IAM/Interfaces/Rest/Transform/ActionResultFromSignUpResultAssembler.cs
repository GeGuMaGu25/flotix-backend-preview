using FlotixPlatform.API.IAM.Application.Errors;
using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to sign up a user in the application layer.
/// </summary>
public static class ActionResultFromSignUpResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a sign-up operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to sign up a user.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getUserByIdActionName">The name of the action used to generate the location header.</param>
    /// <param name="usersControllerName">The name of the controller that exposes the user.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromSignUpResult(
        Result<User, SignUpError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getUserByIdActionName,
        string usersControllerName) =>
        result switch
        {
            Result<User, SignUpError>.Success success =>
                controller.CreatedAtAction(getUserByIdActionName, usersControllerName, new { id = success.Value.Id },
                    UserResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<User, SignUpError>.Failure failure =>
                failure.Error switch
                {
                    SignUpError.InvalidUser =>
                        controller.BadRequest(localizer["InvalidSignUpRequest"].Value),

                    SignUpError.DuplicateUserEmail =>
                        controller.Conflict(localizer["DuplicateUserEmail"].Value),

                    SignUpError.DriverLicenseRequired =>
                        controller.BadRequest(localizer["DriverLicenseRequired"].Value),

                    SignUpError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorSigningUp"].Value,
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
