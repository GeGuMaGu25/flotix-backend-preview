using FlotixPlatform.API.Commerce.Domain.Model.Aggregates;
using FlotixPlatform.API.Commerce.Domain.Model.ValueObjects;
using FlotixPlatform.API.Commerce.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Commerce.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for IoT catalog item persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with IoT catalog item-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class IotCatalogItemRepository(AppDbContext context)
    : BaseRepository<IotCatalogItem>(context), IIotCatalogItemRepository
{
    /// <inheritdoc />
    public async Task<bool> ExistsBySkuAsync(Sku sku, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotCatalogItem>().AnyAsync(i => i.Sku == sku, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IotCatalogItem?> FindBySkuAsync(Sku sku, CancellationToken cancellationToken = default)
    {
        return await Context.Set<IotCatalogItem>().FirstOrDefaultAsync(i => i.Sku == sku, cancellationToken);
    }
}
