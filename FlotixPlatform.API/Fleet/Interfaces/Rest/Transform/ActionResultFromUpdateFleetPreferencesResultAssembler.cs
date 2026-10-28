using FlotixPlatform.API.Fleet.Application.Errors;
using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Fleet.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update the fleet preferences in the application layer.
/// </summary>
public static class ActionResultFromUpdateFleetPreferencesResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of an update fleet preferences operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update the fleet preferences.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateFleetPreferencesResult(
        Result<FleetPreferences, UpdateFleetPreferencesError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<FleetPreferences, UpdateFleetPreferencesError>.Success success =>
                controller.Ok(FleetPreferencesResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<FleetPreferences, UpdateFleetPreferencesError>.Failure failure =>
                failure.Error switch
                {
                    UpdateFleetPreferencesError.FleetPreferencesNotFound =>
                        controller.NotFound(localizer["FleetPreferencesNotFound"].Value),

                    UpdateFleetPreferencesError.InvalidFleetPreferences =>
                        controller.BadRequest(localizer["InvalidFleetPreferencesRequest"].Value),

                    UpdateFleetPreferencesError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingFleetPreferences"].Value,
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
