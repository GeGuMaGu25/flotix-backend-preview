using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Application.Services;
using FlotixPlatform.API.Notifications.Domain.Model.Queries;
using FlotixPlatform.API.Notifications.Domain.Repositories;

namespace FlotixPlatform.API.Notifications.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying notification preferences.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving notification preferences.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="notificationPreferenceRepository">Repository for accessing notification preference data.</param>
public class NotificationPreferenceQueryService(INotificationPreferenceRepository notificationPreferenceRepository)
    : INotificationPreferenceQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<NotificationPreference>> Handle(GetAllNotificationPreferencesQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationPreferenceRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<NotificationPreference?> Handle(GetNotificationPreferenceByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationPreferenceRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<NotificationPreference>> Handle(GetAllNotificationPreferencesByUserIdQuery query,
        CancellationToken cancellationToken = default) =>
        await notificationPreferenceRepository.FindByUserIdAsync(query.UserId, cancellationToken);
}
