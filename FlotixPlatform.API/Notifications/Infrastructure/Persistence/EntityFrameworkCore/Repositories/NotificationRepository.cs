using FlotixPlatform.API.Notifications.Domain.Model.Aggregates;
using FlotixPlatform.API.Notifications.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.Notifications.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for notification persistence.
/// </summary>
/// <remarks>
///     This repository extends the generic BaseRepository with notification-specific query methods.
/// </remarks>
/// <param name="context">The EF Core database context.</param>
public class NotificationRepository(AppDbContext context)
    : BaseRepository<Notification>(context), INotificationRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Notification>> FindByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Notification>()
            .Where(n => n.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
