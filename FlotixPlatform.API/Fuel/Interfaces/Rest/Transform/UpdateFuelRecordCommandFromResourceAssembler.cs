using FlotixPlatform.API.Fuel.Domain.Model.Commands;
using FlotixPlatform.API.Fuel.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Fuel.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateFuelRecordCommand from an UpdateFuelRecordResource.
/// </summary>
public static class UpdateFuelRecordCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateFuelRecordCommand from an UpdateFuelRecordResource.
    /// </summary>
    /// <param name="fuelRecordId">The identifier of the fuel record to update</param>
    /// <param name="resource">The UpdateFuelRecordResource resource</param>
    /// <returns>
    ///     An UpdateFuelRecordCommand assembled from the UpdateFuelRecordResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateFuelRecordCommand ToCommandFromResource(int fuelRecordId, UpdateFuelRecordResource resource) =>
        new(fuelRecordId, resource.Liters, resource.TotalCost, resource.MileageAtRefuel, resource.Date);
}
