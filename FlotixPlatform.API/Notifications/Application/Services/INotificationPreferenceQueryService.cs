using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Domain.Model.Queries;

namespace FlotixPlatform.API.Notifications.Application.Services;

/// <summary>
///     Query service interface for notification preference operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for querying notification preferences by various filters.
/// </remarks>
public interface INotificationPreferenceQueryService
{
	/// <summary>
	///     Handle the GetAllNotificationPreferencesQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllNotificationPreferencesQuery. It returns all the notification preferences.
	/// </remarks>
	/// <param name="query">The GetAllNotificationPreferencesQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the NotificationPreference objects</returns>
	Task<IEnumerable<NotificationPreference>> Handle(GetAllNotificationPreferencesQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetNotificationPreferenceByIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetNotificationPreferenceByIdQuery. It returns the notification preference for the given Id.
	/// </remarks>
	/// <param name="query">The GetNotificationPreferenceByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The NotificationPreference object if found, or null otherwise</returns>
	Task<NotificationPreference?> Handle(GetNotificationPreferenceByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllNotificationPreferencesByUserIdQuery.
	/// </summary>
	/// <remarks>
	///     This method handles the GetAllNotificationPreferencesByUserIdQuery. It returns all the notification preferences for the given user ID.
	/// </remarks>
	/// <param name="query">The GetAllNotificationPreferencesByUserIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the NotificationPreference objects</returns>
	Task<IEnumerable<NotificationPreference>> Handle(GetAllNotificationPreferencesByUserIdQuery query, CancellationToken cancellationToken = default);
}
