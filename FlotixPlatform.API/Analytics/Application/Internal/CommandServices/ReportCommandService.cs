using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.API.Analytics.Application.Errors;
using FlotixPlatform.API.Analytics.Application.Services;
using FlotixPlatform.API.Analytics.Domain.Model.Commands;
using FlotixPlatform.API.Analytics.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Analytics.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling report commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for reports.
///     It enforces the application rules, delegates the business rules to the aggregate,
///     then coordinates with the unit of work to persist changes.
///     Logs warnings for rejected commands and errors for persistence failures.
/// </remarks>
/// <param name="reportRepository">Repository for accessing report data.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
/// See
/// <see cref="IReportRepository">IReportRepository</see>
/// ,
/// <see cref="IUnitOfWork">IUnitOfWork</see>
public class ReportCommandService(
    IReportRepository reportRepository,
    IUnitOfWork unitOfWork,
    ILogger<ReportCommandService> logger)
    : IReportCommandService
{
    /// <inheritdoc />
    public async Task<Result<Report, CreateReportError>> Handle(CreateReportCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var report = new Report(command);
            await reportRepository.AddAsync(report, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Report, CreateReportError>.Success(report);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating report");
            return new Result<Report, CreateReportError>.Failure(CreateReportError.InvalidReport);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation creating report");
            return new Result<Report, CreateReportError>.Failure(CreateReportError.ReferencedResourceNotFound);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating report");
            return new Result<Report, CreateReportError>.Failure(CreateReportError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating report");
            return new Result<Report, CreateReportError>.Failure(CreateReportError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Report, DeleteReportError>> Handle(DeleteReportCommand command,
        CancellationToken cancellationToken = default)
    {
        var report = await reportRepository.FindByIdAsync(command.ReportId, cancellationToken);
        if (report is null)
            return new Result<Report, DeleteReportError>.Failure(
                DeleteReportError.ReportNotFound);

        try
        {
            reportRepository.Remove(report);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<Report, DeleteReportError>.Success(report);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting report");
            return new Result<Report, DeleteReportError>.Failure(DeleteReportError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting report");
            return new Result<Report, DeleteReportError>.Failure(DeleteReportError.UnexpectedError);
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
