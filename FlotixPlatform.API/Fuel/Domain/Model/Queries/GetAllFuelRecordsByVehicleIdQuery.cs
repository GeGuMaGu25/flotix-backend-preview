namespace FlotixPlatform.API.Fuel.Domain.Model.Queries;

/// <summary>
///     Query to get all fuel records by vehicle ID
/// </summary>
/// <param name="VehicleId">The vehicle ID to search</param>
public record GetAllFuelRecordsByVehicleIdQuery(int VehicleId);
