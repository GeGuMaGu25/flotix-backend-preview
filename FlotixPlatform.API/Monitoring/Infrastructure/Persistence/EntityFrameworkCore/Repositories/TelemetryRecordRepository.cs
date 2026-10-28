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
public class TelemetryRecordRepository(AppDbContext context)
    : BaseRepository<TelemetryRecord>(context), ITelemetryRecordRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<TelemetryRecord>> FindByDeviceIdAsync(int deviceId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TelemetryRecord>()
            .Where(t => t.DeviceId == deviceId)
            .ToListAsync(cancellationToken);
    }
}
