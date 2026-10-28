namespace FlotixPlatform.API.Notifications.Domain.Model.Queries;

/// <summary>
///     Query to get a notification preference by ID
/// </summary>
/// <param name="Id">The notification preference ID to search</param>
public record GetNotificationPreferenceByIdQuery(int Id);
