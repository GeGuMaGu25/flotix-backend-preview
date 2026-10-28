using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a WorkshopResource from a Workshop.
/// </summary>
public static class WorkshopResourceFromEntityAssembler
{
    /// <summary>
    ///     Assembles a WorkshopResource from a Workshop.
    /// </summary>
    /// <param name="entity">The Workshop entity</param>
    /// <returns>
    ///     A WorkshopResource assembled from the Workshop
    /// </returns>
    public static WorkshopResource ToResourceFromEntity(Workshop entity) =>
        new(entity.Id, entity.MechanicId, entity.City, entity.District, entity.Address, entity.Phone.Value, entity.Rating, entity.Tags);
}
