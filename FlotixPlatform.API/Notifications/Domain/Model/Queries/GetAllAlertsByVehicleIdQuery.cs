namespace FlotixPlatform.API.Notifications.Domain.Model.Queries;

/// <summary>
///     Query to get all alerts by vehicle ID
/// </summary>
/// <param name="VehicleId">The vehicle ID to search</param>
public record GetAllAlertsByVehicleIdQuery(int VehicleId);
