using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Application.Errors;
using FlotixPlatform.API.Monitoring.Application.Services;
using FlotixPlatform.API.Monitoring.Domain.Model.Commands;
using FlotixPlatform.API.Monitoring.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Monitoring.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling telemetry record commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for telemetry records.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="telemetryDataRepository">Repository for accessing telemetry record data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="ITelemetryDataRepository">ITelemetryDataRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class TelemetryDataCommandService(
    ITelemetryDataRepository telemetryDataRepository,
    IUnitOfWork unitOfWork,
    ILogger<TelemetryDataCommandService> logger)
    : ITelemetryDataCommandService
{
    /// <inheritdoc />
    public async Task<Result<TelemetryData, CreateTelemetryDataError>> Handle(CreateTelemetryDataCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var telemetryData = new TelemetryData(command);
            await telemetryDataRepository.AddAsync(telemetryData, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<TelemetryData, CreateTelemetryDataError>.Success(telemetryData);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating telemetry record");
            return new Result<TelemetryData, CreateTelemetryDataError>.Failure(CreateTelemetryDataError.InvalidTelemetryData);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating telemetry record");
            return new Result<TelemetryData, CreateTelemetryDataError>.Failure(CreateTelemetryDataError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating telemetry record");
            return new Result<TelemetryData, CreateTelemetryDataError>.Failure(CreateTelemetryDataError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating telemetry record");
            return new Result<TelemetryData, CreateTelemetryDataError>.Failure(CreateTelemetryDataError.UnexpectedError);
        }
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a foreign key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL foreign key violation (SQLSTATE 23503), false otherwise.</returns>
    private static bool IsForeignKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23503")
                return true;
        }
        return false;
    }
}
