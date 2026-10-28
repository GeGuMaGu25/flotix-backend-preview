using FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;
using FlotixPlatform.API.Maintenance.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Maintenance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for workshop persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with workshop-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class WorkshopRepository(AppDbContext context)
    : BaseRepository<Workshop>(context), IWorkshopRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Workshop>> FindByMechanicIdAsync(int mechanicId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Workshop>()
            .Where(w => w.MechanicId == mechanicId)
            .ToListAsync(cancellationToken);
    }
}
