using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Transform;

/// <summary>
///     Assembles an UpdateMaintenanceRequestCommand from an UpdateMaintenanceRequestResource.
/// </summary>
public static class UpdateMaintenanceRequestCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles an UpdateMaintenanceRequestCommand from an UpdateMaintenanceRequestResource.
    /// </summary>
    /// <param name="maintenanceRequestId">The identifier of the maintenance request to update</param>
    /// <param name="resource">The UpdateMaintenanceRequestResource resource</param>
    /// <returns>
    ///     An UpdateMaintenanceRequestCommand assembled from the UpdateMaintenanceRequestResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when a value object cannot be created from the resource.</exception>
    public static UpdateMaintenanceRequestCommand ToCommandFromResource(int maintenanceRequestId, UpdateMaintenanceRequestResource resource) =>
        new(maintenanceRequestId, resource.MechanicId, resource.FaultDescription, resource.EstimatedCost, resource.Status);
}
