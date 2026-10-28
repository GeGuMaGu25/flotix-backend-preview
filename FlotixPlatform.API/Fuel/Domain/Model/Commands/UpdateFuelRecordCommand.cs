namespace FlotixPlatform.API.Fuel.Domain.Model.Commands;

/// <summary>
///     Command to update a fuel record
/// </summary>
/// <param name="FuelRecordId">The identifier of the fuel record to update</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="TotalCost">The total cost of the refuel.</param>
/// <param name="MileageAtRefuel">The vehicle mileage when refueling.</param>
/// <param name="Date">The date of the refuel.</param>
public record UpdateFuelRecordCommand(int FuelRecordId, decimal Liters, decimal TotalCost, int MileageAtRefuel, DateOnly Date);
