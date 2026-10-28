using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.Notifications.Application.Services;

/// <summary>
///     Command service interface for notification preference operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for handling the commands that change notification preferences.
/// </remarks>
public interface INotificationPreferenceCommandService
{
    /// <summary>
    ///     Handle the create notification preference command.
    /// </summary>
    /// <param name="command">CreateNotificationPreferenceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created NotificationPreference on success, or the error reason on failure.
    /// </returns>
    Task<Result<NotificationPreference, CreateNotificationPreferenceError>> Handle(CreateNotificationPreferenceCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update notification preference command.
    /// </summary>
    /// <param name="command">UpdateNotificationPreferenceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated NotificationPreference on success, or the error reason on failure.
    /// </returns>
    Task<Result<NotificationPreference, UpdateNotificationPreferenceError>> Handle(UpdateNotificationPreferenceCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete notification preference command.
    /// </summary>
    /// <param name="command">DeleteNotificationPreferenceCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted NotificationPreference on success, or the error reason on failure.
    /// </returns>
    Task<Result<NotificationPreference, DeleteNotificationPreferenceError>> Handle(DeleteNotificationPreferenceCommand command, CancellationToken cancellationToken = default);
}
