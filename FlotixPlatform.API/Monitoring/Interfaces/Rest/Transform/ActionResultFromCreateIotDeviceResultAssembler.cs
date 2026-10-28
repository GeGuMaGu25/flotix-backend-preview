using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Monitoring.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create an IoT device in the application layer.
/// </summary>
public static class ActionResultFromCreateIotDeviceResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create IoT device operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create an IoT device.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getIotDeviceByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateIotDeviceResult(
        Result<IotDevice, CreateIotDeviceError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getIotDeviceByIdActionName) =>
        result switch
        {
            Result<IotDevice, CreateIotDeviceError>.Success success =>
                controller.CreatedAtAction(getIotDeviceByIdActionName, new { id = success.Value.Id },
                    IotDeviceResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotDevice, CreateIotDeviceError>.Failure failure =>
                failure.Error switch
                {
                    CreateIotDeviceError.InvalidIotDevice =>
                        controller.BadRequest(localizer["InvalidIotDeviceRequest"].Value),
                    CreateIotDeviceError.DuplicateIotDeviceSerialNumber =>
                        controller.Conflict(localizer["DuplicateIotDeviceSerialNumber"].Value),
                    CreateIotDeviceError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingIotDevice"].Value,
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
