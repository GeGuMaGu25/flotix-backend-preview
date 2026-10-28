using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Application.Errors;
using FlotixPlatform.API.Notifications.Application.Services;
using FlotixPlatform.API.Notifications.Domain.Model.Commands;
using FlotixPlatform.API.Notifications.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Notifications.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling notification preference commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for notification preferences.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="notificationPreferenceRepository">Repository for accessing notification preference data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="INotificationPreferenceRepository">INotificationPreferenceRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class NotificationPreferenceCommandService(
    INotificationPreferenceRepository notificationPreferenceRepository,
    IUnitOfWork unitOfWork,
    ILogger<NotificationPreferenceCommandService> logger)
    : INotificationPreferenceCommandService
{
    /// <inheritdoc />
    public async Task<Result<NotificationPreference, CreateNotificationPreferenceError>> Handle(CreateNotificationPreferenceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await notificationPreferenceRepository.ExistsByUserIdAsync(command.UserId, cancellationToken))
        {
            logger.LogWarning("Duplicate notification preference rejected for UserId {UserId}", command.UserId);
            return new Result<NotificationPreference, CreateNotificationPreferenceError>.Failure(
                CreateNotificationPreferenceError.DuplicateNotificationPreferenceUserId);
        }

        try
        {
            var notificationPreference = new NotificationPreference(command);
            await notificationPreferenceRepository.AddAsync(notificationPreference, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<NotificationPreference, CreateNotificationPreferenceError>.Success(notificationPreference);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating notification preference");
            return new Result<NotificationPreference, CreateNotificationPreferenceError>.Failure(CreateNotificationPreferenceError.InvalidNotificationPreference);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation creating notification preference");
            return new Result<NotificationPreference, CreateNotificationPreferenceError>.Failure(CreateNotificationPreferenceError.DuplicateNotificationPreferenceUserId);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating notification preference");
            return new Result<NotificationPreference, CreateNotificationPreferenceError>.Failure(CreateNotificationPreferenceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating notification preference");
            return new Result<NotificationPreference, CreateNotificationPreferenceError>.Failure(CreateNotificationPreferenceError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<NotificationPreference, UpdateNotificationPreferenceError>> Handle(UpdateNotificationPreferenceCommand command,
        CancellationToken cancellationToken = default)
    {
        var notificationPreference = await notificationPreferenceRepository.FindByIdAsync(command.NotificationPreferenceId, cancellationToken);
        if (notificationPreference is null)
            return new Result<NotificationPreference, UpdateNotificationPreferenceError>.Failure(
                UpdateNotificationPreferenceError.NotificationPreferenceNotFound);

        try
        {
            notificationPreference.UpdateInformation(command);
            notificationPreferenceRepository.Update(notificationPreference);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<NotificationPreference, UpdateNotificationPreferenceError>.Success(notificationPreference);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating notification preference");
            return new Result<NotificationPreference, UpdateNotificationPreferenceError>.Failure(UpdateNotificationPreferenceError.InvalidNotificationPreference);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating notification preference");
            return new Result<NotificationPreference, UpdateNotificationPreferenceError>.Failure(UpdateNotificationPreferenceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating notification preference");
            return new Result<NotificationPreference, UpdateNotificationPreferenceError>.Failure(UpdateNotificationPreferenceError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<NotificationPreference, DeleteNotificationPreferenceError>> Handle(DeleteNotificationPreferenceCommand command,
        CancellationToken cancellationToken = default)
    {
        var notificationPreference = await notificationPreferenceRepository.FindByIdAsync(command.NotificationPreferenceId, cancellationToken);
        if (notificationPreference is null)
            return new Result<NotificationPreference, DeleteNotificationPreferenceError>.Failure(
                DeleteNotificationPreferenceError.NotificationPreferenceNotFound);

        try
        {
            notificationPreferenceRepository.Remove(notificationPreference);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<NotificationPreference, DeleteNotificationPreferenceError>.Success(notificationPreference);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting notification preference");
            return new Result<NotificationPreference, DeleteNotificationPreferenceError>.Failure(DeleteNotificationPreferenceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting notification preference");
            return new Result<NotificationPreference, DeleteNotificationPreferenceError>.Failure(DeleteNotificationPreferenceError.UnexpectedError);
        }
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a duplicate key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL unique violation (SQLSTATE 23505), false otherwise.</returns>
    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23505")
                return true;
        }
        return false;
    }
}
