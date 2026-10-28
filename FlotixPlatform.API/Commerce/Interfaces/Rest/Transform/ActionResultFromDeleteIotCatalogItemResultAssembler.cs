using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to delete an IoT catalog item in the application layer.
/// </summary>
public static class ActionResultFromDeleteIotCatalogItemResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a delete IoT catalog item operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to delete an IoT catalog item.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromDeleteIotCatalogItemResult(
        Result<IotCatalogItem, DeleteIotCatalogItemError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer) =>
        result switch
        {
            Result<IotCatalogItem, DeleteIotCatalogItemError>.Success success =>
                controller.NoContent(),

            Result<IotCatalogItem, DeleteIotCatalogItemError>.Failure failure =>
                failure.Error switch
                {
                    DeleteIotCatalogItemError.IotCatalogItemNotFound =>
                        controller.NotFound(localizer["IotCatalogItemNotFound"].Value),
                    DeleteIotCatalogItemError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorDeletingIotCatalogItem"].Value,
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
