using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update an IoT device in the application layer.
/// </summary>
public static class ActionResultFromUpdateIotDeviceResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update IoT device operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update an IoT device.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateIotDeviceResult(
        Result<IotDevice, UpdateIotDeviceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<IotDevice, UpdateIotDeviceError>.Success success =>
                controller.Ok(IotDeviceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotDevice, UpdateIotDeviceError>.Failure failure =>
                failure.Error switch
                {
                    UpdateIotDeviceError.IotDeviceNotFound =>
                        controller.NotFound(localizer["IotDeviceNotFound"].Value),
                    UpdateIotDeviceError.InvalidIotDevice =>
                        controller.BadRequest(localizer["InvalidIotDeviceRequest"].Value),
                    UpdateIotDeviceError.DuplicateIotDeviceVehicleId =>
                        controller.Conflict(localizer["DuplicateIotDeviceVehicleId"].Value),
                    UpdateIotDeviceError.ReferencedResourceNotFound =>
                        controller.NotFound(localizer["IotDeviceReferencedResourceNotFound"].Value),
                    UpdateIotDeviceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingIotDevice"].Value,
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
