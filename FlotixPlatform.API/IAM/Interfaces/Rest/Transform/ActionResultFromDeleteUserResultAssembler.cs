using FlotixPlatform.API.IAM.Application.Errors;
using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to delete a user in the application layer.
/// </summary>
public static class ActionResultFromDeleteUserResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a delete user operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to delete a user.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromDeleteUserResult(
        Result<User, DeleteUserError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<User, DeleteUserError>.Success success =>
                controller.NoContent(),

            Result<User, DeleteUserError>.Failure failure =>
                failure.Error switch
                {
                    DeleteUserError.UserNotFound =>
                        controller.NotFound(localizer["UserNotFound"].Value),

                    DeleteUserError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingUser"].Value,
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
