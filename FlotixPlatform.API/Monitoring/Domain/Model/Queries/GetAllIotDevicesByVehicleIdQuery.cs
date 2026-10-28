namespace FlotixPlatform.API.Monitoring.Domain.Model.Queries;

/// <summary>
///     Query to get all IoT devices by vehicle ID
/// </summary>
/// <param name="VehicleId">The vehicle ID to search</param>
public record GetAllIotDevicesByVehicleIdQuery(int VehicleId);
