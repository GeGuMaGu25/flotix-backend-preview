using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.IAM.Domain.Repositories;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using FlotixPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.IAM.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework repository for user persistence.
/// </summary>
/// <param name="context">The EF Core database context.</param>
public class UserRepository(AppDbContext context)
    : BaseRepository<User>(context), IUserRepository
{
    /// <inheritdoc />
    public async Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>().AnyAsync(u => u.Email == email, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<User>> FindByRoleTypeAsync(Role roleType, CancellationToken cancellationToken = default)
    {
        return await Context.Set<User>()
            .Where(u => u.RoleType == roleType)
            .ToListAsync(cancellationToken);
    }
}
