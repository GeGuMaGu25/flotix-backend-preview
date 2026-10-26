using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Fleet.Domain;

public sealed class Vehicle : Entity
{
    private Vehicle() { }

    public Vehicle(string ownerId, string? driverId, string plate, string vehicleType, int currentMileage, VehicleStatus status)
    {
        OwnerId = ownerId;
        Update(driverId, plate, vehicleType, currentMileage, status);
    }

    public string OwnerId { get; private set; } = null!;

    public string? DriverId { get; private set; }

    public string Plate { get; private set; } = null!;

    public string VehicleType { get; private set; } = null!;

    public int CurrentMileage { get; private set; }

    public VehicleStatus Status { get; private set; }

    public void Update(string? driverId, string plate, string vehicleType, int currentMileage, VehicleStatus status)
    {
        if (currentMileage < CurrentMileage) throw new DomainException("The mileage of a vehicle cannot decrease.");

        DriverId = driverId;
        Plate = plate.Trim().ToUpperInvariant();
        VehicleType = vehicleType;
        CurrentMileage = currentMileage;
        Status = status;
    }
}
