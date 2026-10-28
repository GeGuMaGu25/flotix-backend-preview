using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.API.Analytics.Application.Services;
using FlotixPlatform.API.Analytics.Domain.Model.Queries;
using FlotixPlatform.API.Analytics.Domain.Repositories;

namespace FlotixPlatform.API.Analytics.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying reports.
/// </summary>
/// <remarks>
///     This service acts as the query handler for retrieving reports.
///     All queries delegate to the repository layer for data access.
/// </remarks>
/// <param name="reportRepository">Repository for accessing report data.</param>
public class ReportQueryService(IReportRepository reportRepository)
    : IReportQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Report>> Handle(GetAllReportsQuery query,
        CancellationToken cancellationToken = default) =>
        await reportRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<Report?> Handle(GetReportByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await reportRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<Report>> Handle(GetAllReportsByOwnerIdQuery query,
        CancellationToken cancellationToken = default) =>
        await reportRepository.FindByOwnerIdAsync(query.OwnerId, cancellationToken);
}
