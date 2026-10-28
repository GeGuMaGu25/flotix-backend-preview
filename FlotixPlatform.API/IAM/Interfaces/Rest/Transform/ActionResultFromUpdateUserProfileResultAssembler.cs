using FlotixPlatform.API.IAM.Application.Errors;
using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update the profile of a user in the application layer.
/// </summary>
public static class ActionResultFromUpdateUserProfileResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of an update user profile operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update the profile of a user.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateUserProfileResult(
        Result<User, UpdateUserProfileError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<User, UpdateUserProfileError>.Success success =>
                controller.Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<User, UpdateUserProfileError>.Failure failure =>
                failure.Error switch
                {
                    UpdateUserProfileError.UserNotFound =>
                        controller.NotFound(localizer["UserNotFound"].Value),

                    UpdateUserProfileError.InvalidUser =>
                        controller.BadRequest(localizer["InvalidUserRequest"].Value),

                    UpdateUserProfileError.DriverLicenseRequired =>
                        controller.BadRequest(localizer["DriverLicenseRequired"].Value),

                    UpdateUserProfileError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingUser"].Value,
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
