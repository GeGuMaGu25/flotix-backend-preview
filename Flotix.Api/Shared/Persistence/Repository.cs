using System.Linq.Expressions;
using Flotix.Api.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Flotix.Api.Shared.Persistence;

/// <summary>Generic EF Core repository shared by every bounded context.</summary>
public sealed class Repository<T>(AppDbContext context) : IRepository<T> where T : Entity
{
    public async Task<T?> FindAsync(string id, CancellationToken cancellationToken = default)
        => await context.Set<T>().FindAsync([id], cancellationToken);

    public Task<List<T>> ListAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        => context.Set<T>().AsNoTracking().Where(filter ?? (_ => true)).OrderBy(e => e.CreatedAt).ToListAsync(cancellationToken);

    public void Add(T entity) => context.Set<T>().Add(entity);

    public void Remove(T entity) => context.Set<T>().Remove(entity);
}
