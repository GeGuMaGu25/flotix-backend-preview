using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for telemetry record persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with telemetry record-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class TelemetryDataRepository(AppDbContext context)
    : BaseRepository<TelemetryData>(context), ITelemetryDataRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<TelemetryData>> FindByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetryData>()
            .Where(t => t.DeviceId == deviceId)
            .OrderByDescending(t => t.RecordedAt)
            .ToListAsync(cancellationToken);
    }
}
