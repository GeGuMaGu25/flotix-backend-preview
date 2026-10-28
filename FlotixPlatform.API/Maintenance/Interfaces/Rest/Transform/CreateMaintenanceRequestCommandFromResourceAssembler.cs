using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a CreateMaintenanceRequestCommand from a CreateMaintenanceRequestResource.
/// </summary>
public static class CreateMaintenanceRequestCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a CreateMaintenanceRequestCommand from a CreateMaintenanceRequestResource.
    /// </summary>
    /// <param name="resource">The CreateMaintenanceRequestResource resource</param>
    /// <returns>
    ///     A CreateMaintenanceRequestCommand assembled from the CreateMaintenanceRequestResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static CreateMaintenanceRequestCommand ToCommandFromResource(CreateMaintenanceRequestResource resource) =>
        new(resource.VehicleId, resource.MechanicId, resource.FaultDescription, resource.EstimatedCost, resource.Status);
}
