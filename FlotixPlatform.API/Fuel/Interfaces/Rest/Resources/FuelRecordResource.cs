namespace FlotixPlatform.API.Fuel.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a fuel record.
/// </summary>
/// <param name="Id">The server-generated ID of the fuel record</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="DriverId">The identifier of the driver user.</param>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="TotalCost">The total cost of the refuel.</param>
/// <param name="MileageAtRefuel">The vehicle mileage when refueling.</param>
/// <param name="Date">The date of the refuel.</param>
public record FuelRecordResource(
    int Id,
    int VehicleId,
    int DriverId,
    decimal Liters,
    decimal TotalCost,
    int MileageAtRefuel,
    DateOnly Date);
