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
/// <param name="telemetryRecordRepository">Repository for accessing telemetry record data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="ITelemetryRecordRepository">ITelemetryRecordRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class TelemetryRecordCommandService(
    ITelemetryRecordRepository telemetryRecordRepository,
    IUnitOfWork unitOfWork,
    ILogger<TelemetryRecordCommandService> logger)
    : ITelemetryRecordCommandService
{
    /// <inheritdoc />
    public async Task<Result<TelemetryRecord, CreateTelemetryRecordError>> Handle(CreateTelemetryRecordCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var telemetryRecord = new TelemetryRecord(command);
            await telemetryRecordRepository.AddAsync(telemetryRecord, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<TelemetryRecord, CreateTelemetryRecordError>.Success(telemetryRecord);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating telemetry record");
            return new Result<TelemetryRecord, CreateTelemetryRecordError>.Failure(CreateTelemetryRecordError.InvalidTelemetryRecord);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating telemetry record");
            return new Result<TelemetryRecord, CreateTelemetryRecordError>.Failure(CreateTelemetryRecordError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating telemetry record");
            return new Result<TelemetryRecord, CreateTelemetryRecordError>.Failure(CreateTelemetryRecordError.UnexpectedError);
        }
    }
}
