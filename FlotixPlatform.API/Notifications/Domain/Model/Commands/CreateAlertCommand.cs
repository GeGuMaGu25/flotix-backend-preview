namespace FlotixPlatform.API.Notifications.Domain.Model.Commands;

/// <summary>
///     Command to create an alert
/// </summary>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="AlertType">The type of the alert (e.g. Exceso de velocidad).</param>
public record CreateAlertCommand(int VehicleId, string AlertType);
