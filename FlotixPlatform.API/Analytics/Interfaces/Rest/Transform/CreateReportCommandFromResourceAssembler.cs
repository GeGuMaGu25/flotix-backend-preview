using FlotixPlatform.API.Analytics.Domain.Model.Commands;
using FlotixPlatform.API.Analytics.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateReportCommand from a CreateReportResource.
/// </summary>
public static class CreateReportCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateReportCommand from a CreateReportResource.
    /// </summary>
    /// <param name="resource">The CreateReportResource resource</param>
    /// <returns>
    ///     A CreateReportCommand assembled from the CreateReportResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateReportCommand ToCommandFromResource(CreateReportResource resource) =>
        new(resource.OwnerId, resource.ReportType, resource.FilterParams);
}
