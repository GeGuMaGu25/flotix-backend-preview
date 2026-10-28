using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.Notifications.Domain.Repositories;

/// <summary>
///     The notification repository contract.
/// </summary>
public interface INotificationRepository : IBaseRepository<Notification>
{
    /// <summary>
    ///     Find notifications by user ID
    /// </summary>
    /// <param name="userId">The user ID to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the notification objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<Notification>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}
