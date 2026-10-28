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
///     Application service for handling alert commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for alerts.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="alertRepository">Repository for accessing alert data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IAlertRepository">IAlertRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class AlertCommandService(
    IAlertRepository alertRepository,
    IUnitOfWork unitOfWork,
    ILogger<AlertCommandService> logger)
    : IAlertCommandService
{
    /// <inheritdoc />
    public async Task<Result<Alert, CreateAlertError>> Handle(CreateAlertCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var alert = new Alert(command);
            await alertRepository.AddAsync(alert, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Alert, CreateAlertError>.Success(alert);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating alert");
            return new Result<Alert, CreateAlertError>.Failure(CreateAlertError.InvalidAlert);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating alert");
            return new Result<Alert, CreateAlertError>.Failure(CreateAlertError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating alert");
            return new Result<Alert, CreateAlertError>.Failure(CreateAlertError.UnexpectedError);
        }
    }
}
