namespace FlotixPlatform.API.Notifications.Domain.Model.Queries;

/// <summary>
///     Query to get all notification preferences by user ID
/// </summary>
/// <param name="UserId">The user ID to search</param>
public record GetAllNotificationPreferencesByUserIdQuery(int UserId);
