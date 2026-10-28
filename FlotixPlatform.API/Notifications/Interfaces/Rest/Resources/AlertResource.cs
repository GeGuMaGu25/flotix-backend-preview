namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about an alert.
/// </summary>
/// <param name="Id">The server-generated ID of the alert</param>
/// <param name="VehicleId">The identifier of the vehicle.</param>
/// <param name="AlertType">The type of the alert (e.g. Exceso de velocidad).</param>
/// <param name="ActivatedAt">The moment the alert was activated.</param>
public record AlertResource(
    int Id,
    int VehicleId,
    string AlertType,
    DateTimeOffset ActivatedAt);
