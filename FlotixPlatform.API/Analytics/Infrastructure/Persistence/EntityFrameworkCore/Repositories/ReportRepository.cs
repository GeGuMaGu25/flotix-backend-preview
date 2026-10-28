using FlotixPlatform.API.Analytics.Domain.Model.Aggregates;
using FlotixPlatform.API.Analytics.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for report persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with report-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class ReportRepository(AppDbContext context)
    : BaseRepository<Report>(context), IReportRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Report>> FindByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Report>()
            .Where(r => r.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
