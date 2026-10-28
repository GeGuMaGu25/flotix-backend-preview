using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for notification preference persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with notification preference-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class NotificationPreferenceRepository(AppDbContext context)
    : BaseRepository<NotificationPreference>(context), INotificationPreferenceRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<NotificationPreference>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<NotificationPreference>()
            .Where(n => n.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<NotificationPreference>().AnyAsync(n => n.UserId == userId, cancellationToken);
    }
}
