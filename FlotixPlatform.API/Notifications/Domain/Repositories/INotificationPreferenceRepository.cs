using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Notifications.Domain.Repositories;

/// <summary>
///     The notification preference repository contract.
/// </summary>
public interface INotificationPreferenceRepository : IBaseRepository<NotificationPreference>
{
    /// <summary>
    ///     Find notification preferences by user ID
    /// </summary>
    /// <param name="userId">The user ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the notification preference objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<NotificationPreference>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Check whether a notification preference with the given user ID exists
    /// </summary>
    /// <param name="userId">The user ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     True if a notification preference with the user ID exists, false otherwise.
    /// </returns>
    Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
