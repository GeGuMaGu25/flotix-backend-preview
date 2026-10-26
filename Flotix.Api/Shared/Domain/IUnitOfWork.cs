namespace Flotix.Api.Shared.Domain;

/// <summary>Commits every change tracked during the current request as a single transaction.</summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
