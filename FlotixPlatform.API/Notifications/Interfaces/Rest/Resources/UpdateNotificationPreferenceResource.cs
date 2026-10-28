namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to update a notification preference.
/// </summary>
/// <param name="Maintenance">Whether maintenance notifications are enabled.</param>
/// <param name="Speed">Whether speeding notifications are enabled.</param>
/// <param name="Incidents">Whether incident notifications are enabled.</param>
public record UpdateNotificationPreferenceResource(
    bool Maintenance,
    bool Speed,
    bool Incidents);
