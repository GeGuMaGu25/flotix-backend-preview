using System.Linq.Expressions;

namespace Flotix.Api.Shared.Domain;

/// <summary>Collection-like access to the aggregates of type <typeparamref name="T"/>.</summary>
public interface IRepository<T> where T : Entity
{
    Task<T?> FindAsync(string id, CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

    void Add(T entity);

    void Remove(T entity);
}
