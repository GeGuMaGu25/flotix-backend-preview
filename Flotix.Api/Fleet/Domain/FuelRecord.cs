using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Fleet.Domain;

public sealed class FuelRecord : Entity
{
    private FuelRecord() { }

    public FuelRecord(string vehicleId, string driverId, decimal liters, decimal totalCost, int mileageAtRefuel, DateOnly date)
    {
        VehicleId = vehicleId;
        DriverId = driverId;
        Update(liters, totalCost, mileageAtRefuel, date);
    }

    public string VehicleId { get; private set; } = null!;

    public string DriverId { get; private set; } = null!;

    public decimal Liters { get; private set; }

    public decimal TotalCost { get; private set; }

    public int MileageAtRefuel { get; private set; }

    public DateOnly Date { get; private set; }

    public void Update(decimal liters, decimal totalCost, int mileageAtRefuel, DateOnly date)
    {
        Liters = liters;
        TotalCost = totalCost;
        MileageAtRefuel = mileageAtRefuel;
        Date = date;
    }
}
