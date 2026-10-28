using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Fuel.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a fuel record.
/// </summary>
/// <param name="Liters">The refueled amount in liters.</param>
/// <param name="TotalCost">The total cost of the refuel.</param>
/// <param name="MileageAtRefuel">The vehicle mileage when refueling.</param>
/// <param name="Date">The date of the refuel.</param>
public record UpdateFuelRecordResource(
    [Range(0.01, 10000, ErrorMessage = "RangeField")]
    decimal Liters,
    [Range(0, 1000000, ErrorMessage = "RangeField")]
    decimal TotalCost,
    [Range(0, int.MaxValue, ErrorMessage = "RangeField")]
    int MileageAtRefuel,
    DateOnly Date);
