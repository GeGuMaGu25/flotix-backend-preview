namespace FlotixPlatform.API.Fuel.Domain.Model.Commands;

/// <summary>
///     Command to create a fuel record
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="TotalCost">The total cost of the refuel.</param>
/// <param name="MileageAtRefuel">The vehicle mileage when refueling.</param>
/// <param name="Date">The date of the refuel.</param>
public record CreateFuelRecordCommand(int VehicleId, int DriverId, decimal Liters, decimal TotalCost, int MileageAtRefuel, DateOnly Date);
