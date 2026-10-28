using FlotixPlatform.API.Monitoring.Domain.Model.Aggregates;
using FlotixPlatform.API.Monitoring.Domain.Model.ValueObjects;
using FlotixPlatform.API.Monitoring.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for IoT device persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with IoT device-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class IotDeviceRepository(AppDbContext context)
    : BaseRepository<IotDevice>(context), IIotDeviceRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<IotDevice>> FindByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>()
            .Where(i => i.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>().AnyAsync(i => i.VehicleId == vehicleId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsBySerialNumberAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>().AnyAsync(i => i.SerialNumber == serialNumber, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IotDevice?> FindBySerialNumberAsync(SerialNumber serialNumber, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotDevice>().FirstOrDefaultAsync(i => i.SerialNumber == serialNumber, cancellationToken);
    }
}
