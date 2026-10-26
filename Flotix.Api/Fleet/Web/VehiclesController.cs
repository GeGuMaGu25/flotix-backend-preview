using System.ComponentModel.DataAnnotations;
using Flotix.Api.Fleet.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;

namespace Flotix.Api.Fleet.Web;

public sealed record VehicleRequest(
    string OwnerId,
    [StringLength(12, MinimumLength = 1)] string Plate,
    [StringLength(60, MinimumLength = 1)] string VehicleType,
    [Range(0, int.MaxValue)] int CurrentMileage,
    VehicleStatus Status = VehicleStatus.Available,
    string? DriverId = null) : IResourceRequest<VehicleRequest, Vehicle>
{
    public Vehicle ToEntity() => new(OwnerId, DriverId, Plate, VehicleType, CurrentMileage, Status);

    public void ApplyTo(Vehicle vehicle) => vehicle.Update(DriverId, Plate, VehicleType, CurrentMileage, Status);

    public static VehicleRequest FromEntity(Vehicle vehicle)
        => new(vehicle.OwnerId, vehicle.Plate, vehicle.VehicleType, vehicle.CurrentMileage, vehicle.Status, vehicle.DriverId);
}

public sealed class VehiclesController(IRepository<Vehicle> vehicles, IUnitOfWork unitOfWork)
    : CrudController<Vehicle, VehicleRequest>(vehicles, unitOfWork);
