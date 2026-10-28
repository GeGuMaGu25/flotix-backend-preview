using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Resources;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FlotixPlatform.API.Commerce.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an HTTP action result from the result of the command handling to create an IoT catalog item in the application layer.
/// </summary>
public static class ActionResultFromCreateIotCatalogItemResultAssembler
{
    /// <summary>
    ///     Assembles an <see cref="ActionResult" /> from the result of a create IoT catalog item operation.
    /// </summary>
    /// <param name="result">The application result produced by the application layer after handling the command to create an IoT catalog item.</param>
    /// <param name="controller">The controller used to produce HTTP-specific action results.</param>
    /// <param name="localizer">The string localizer used for API response messages.</param>
    /// <param name="getIotCatalogItemByIdActionName">The name of the action used to generate the location header.</param>
    /// <returns>The HTTP action result corresponding to the application outcome.</returns>
    public static ActionResult ToActionResultFromCreateIotCatalogItemResult(
        Result<IotCatalogItem, CreateIotCatalogItemError> result,
        ControllerBase controller,
        IStringLocalizer<SharedResource> localizer,
        string getIotCatalogItemByIdActionName) =>
        result switch
        {
            Result<IotCatalogItem, CreateIotCatalogItemError>.Success success =>
                controller.CreatedAtAction(getIotCatalogItemByIdActionName, new { id = success.Value.Id },
                    IotCatalogItemResourceFromEntityAssembler.ToResourceFromEntity(success.Value)),

            Result<IotCatalogItem, CreateIotCatalogItemError>.Failure failure =>
                failure.Error switch
                {
                    CreateIotCatalogItemError.InvalidIotCatalogItem =>
                        controller.BadRequest(localizer["InvalidIotCatalogItemRequest"].Value),
                    CreateIotCatalogItemError.DuplicateIotCatalogItemSku =>
                        controller.Conflict(localizer["DuplicateIotCatalogItemSku"].Value),
                    CreateIotCatalogItemError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingIotCatalogItem"].Value,
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
