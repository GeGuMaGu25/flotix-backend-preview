using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Application.Errors;
using FlotixPlatform.API.Commerce.Application.Services;
using FlotixPlatform.API.Commerce.Domain.Model.Commands;
using FlotixPlatform.API.Commerce.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Commerce.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling IoT catalog item commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for IoT catalog items.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="iotCatalogItemRepository">Repository for accessing IoT catalog item data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IIotCatalogItemRepository">IIotCatalogItemRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class IotCatalogItemCommandService(
    IIotCatalogItemRepository iotCatalogItemRepository,
    IUnitOfWork unitOfWork,
    ILogger<IotCatalogItemCommandService> logger)
    : IIotCatalogItemCommandService
{
    /// <inheritdoc />
    public async Task<Result<IotCatalogItem, CreateIotCatalogItemError>> Handle(CreateIotCatalogItemCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await iotCatalogItemRepository.ExistsBySkuAsync(command.Sku, cancellationToken))
        {
            logger.LogWarning("Duplicate IoT catalog item rejected for Sku {Sku}", command.Sku);
            return new Result<IotCatalogItem, CreateIotCatalogItemError>.Failure(
                CreateIotCatalogItemError.DuplicateIotCatalogItemSku);
        }

        try
        {
            var iotCatalogItem = new IotCatalogItem(command);
            await iotCatalogItemRepository.AddAsync(iotCatalogItem, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotCatalogItem, CreateIotCatalogItemError>.Success(iotCatalogItem);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating IoT catalog item");
            return new Result<IotCatalogItem, CreateIotCatalogItemError>.Failure(CreateIotCatalogItemError.InvalidIotCatalogItem);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation creating IoT catalog item");
            return new Result<IotCatalogItem, CreateIotCatalogItemError>.Failure(CreateIotCatalogItemError.DuplicateIotCatalogItemSku);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating IoT catalog item");
            return new Result<IotCatalogItem, CreateIotCatalogItemError>.Failure(CreateIotCatalogItemError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating IoT catalog item");
            return new Result<IotCatalogItem, CreateIotCatalogItemError>.Failure(CreateIotCatalogItemError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<IotCatalogItem, UpdateIotCatalogItemError>> Handle(UpdateIotCatalogItemCommand command,
        CancellationToken cancellationToken = default)
    {
        var iotCatalogItem = await iotCatalogItemRepository.FindByIdAsync(command.IotCatalogItemId, cancellationToken);
        if (iotCatalogItem is null)
            return new Result<IotCatalogItem, UpdateIotCatalogItemError>.Failure(
                UpdateIotCatalogItemError.IotCatalogItemNotFound);

        try
        {
            iotCatalogItem.UpdateInformation(command);
            iotCatalogItemRepository.Update(iotCatalogItem);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotCatalogItem, UpdateIotCatalogItemError>.Success(iotCatalogItem);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating IoT catalog item");
            return new Result<IotCatalogItem, UpdateIotCatalogItemError>.Failure(UpdateIotCatalogItemError.InvalidIotCatalogItem);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating IoT catalog item");
            return new Result<IotCatalogItem, UpdateIotCatalogItemError>.Failure(UpdateIotCatalogItemError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating IoT catalog item");
            return new Result<IotCatalogItem, UpdateIotCatalogItemError>.Failure(UpdateIotCatalogItemError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<IotCatalogItem, DeleteIotCatalogItemError>> Handle(DeleteIotCatalogItemCommand command,
        CancellationToken cancellationToken = default)
    {
        var iotCatalogItem = await iotCatalogItemRepository.FindByIdAsync(command.IotCatalogItemId, cancellationToken);
        if (iotCatalogItem is null)
            return new Result<IotCatalogItem, DeleteIotCatalogItemError>.Failure(
                DeleteIotCatalogItemError.IotCatalogItemNotFound);

        try
        {
            iotCatalogItemRepository.Remove(iotCatalogItem);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<IotCatalogItem, DeleteIotCatalogItemError>.Success(iotCatalogItem);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting IoT catalog item");
            return new Result<IotCatalogItem, DeleteIotCatalogItemError>.Failure(DeleteIotCatalogItemError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting IoT catalog item");
            return new Result<IotCatalogItem, DeleteIotCatalogItemError>.Failure(DeleteIotCatalogItemError.UnexpectedError);
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
