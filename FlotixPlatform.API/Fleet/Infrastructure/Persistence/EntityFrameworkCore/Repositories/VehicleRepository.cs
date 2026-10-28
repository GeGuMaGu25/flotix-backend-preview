using FlotixPlatform.API.Fleet.Domain.Model.Aggregates;
using FlotixPlatform.API.Fleet.Domain.Model.ValueObjects;
using FlotixPlatform.API.Fleet.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for vehicle persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with vehicle-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class VehicleRepository(AppDbContext context)
    : BaseRepository<Vehicle>(context), IVehicleRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> FindByOwnerIdAsync(int ownerId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .Where(v => v.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Vehicle>> FindByDriverIdAsync(int driverId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>()
            .Where(v => v.DriverId == driverId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByPlateAsync(LicensePlate plate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>().AnyAsync(v => v.Plate == plate, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Vehicle?> FindByPlateAsync(LicensePlate plate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>().FirstOrDefaultAsync(v => v.Plate == plate, cancellationToken);
    }
}
