using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to update an IoT catalog item in the application layer.
/// </summary>
public static class ActionResultFromUpdateIotCatalogItemResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a update IoT catalog item operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to update an IoT catalog item.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromUpdateIotCatalogItemResult(
        Result<IotCatalogItem, UpdateIotCatalogItemError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<IotCatalogItem, UpdateIotCatalogItemError>.Success success =>
                controller.Ok(IotCatalogItemResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotCatalogItem, UpdateIotCatalogItemError>.Failure failure =>
                failure.Error switch
                {
                    UpdateIotCatalogItemError.IotCatalogItemNotFound =>
                        controller.NotFound(localizer["IotCatalogItemNotFound"].Value),
                    UpdateIotCatalogItemError.InvalidIotCatalogItem =>
                        controller.BadRequest(localizer["InvalidIotCatalogItemRequest"].Value),
                    UpdateIotCatalogItemError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorUpdatingIotCatalogItem"].Value,
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
