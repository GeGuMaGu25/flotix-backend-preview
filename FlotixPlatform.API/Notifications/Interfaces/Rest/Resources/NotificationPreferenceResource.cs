namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a notification preference.
/// </summary>
/// <param name="Id">The server-generated ID of the notification preference</param>
/// <param name="UserId">The identifier of the user.</param>
/// <param name="Maintenance">Whether maintenance notifications are enabled.</param>
/// <param name="Speed">Whether speeding notifications are enabled.</param>
/// <param name="Incidents">Whether incident notifications are enabled.</param>
public record NotificationPreferenceResource(
    int Id,
    int UserId,
    bool Maintenance,
    bool Speed,
    bool Incidents);
