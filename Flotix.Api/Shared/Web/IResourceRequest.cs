namespace Flotix.Api.Shared.Web;

/// <summary>Write model of a REST resource and its mapping to/from the aggregate.</summary>
public interface IResourceRequest<TSelf, TEntity> where TSelf : IResourceRequest<TSelf, TEntity>
{
    TEntity ToEntity();

    void ApplyTo(TEntity entity);

    static abstract TSelf FromEntity(TEntity entity);
}
