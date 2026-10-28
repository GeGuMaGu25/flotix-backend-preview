using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.Notifications.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to create a notification preference.
/// </summary>
/// <param name="UserId">The identifier of the user.</param>
/// <param name="Maintenance">Whether maintenance notifications are enabled.</param>
/// <param name="Speed">Whether speeding notifications are enabled.</param>
/// <param name="Incidents">Whether incident notifications are enabled.</param>
public record CreateNotificationPreferenceResource(
    [Range(1, int.MaxValue, ErrorMessage = "RangeField")]
    int UserId,
    bool Maintenance,
    bool Speed,
    bool Incidents);
