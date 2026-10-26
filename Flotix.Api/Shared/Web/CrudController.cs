using System.Text.Json;
using System.Text.Json.Nodes;
using Flotix.Api.Shared.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Flotix.Api.Shared.Web;

/// <summary>
/// Template Method for a REST resource with the same contract as the json-server fake API:
/// list (with equality filters), get, create, replace, JSON merge patch and delete.
/// Derived controllers only plug in their aggregate, request and, optionally, the hooks.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public abstract class CrudController<TEntity, TRequest>(IRepository<TEntity> repository, IUnitOfWork unitOfWork) : ControllerBase
    where TEntity : Entity
    where TRequest : IResourceRequest<TRequest, TEntity>
{
    [HttpGet]
    public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken)
        => await repository.ListAsync(QueryFilter.FromQuery<TEntity>(Request.Query, JsonOptions), cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<TEntity>> GetById(string id, CancellationToken cancellationToken)
        => await repository.FindAsync(id, cancellationToken) is { } entity ? entity : NotFound();

    [HttpPost]
    public virtual async Task<ActionResult<TEntity>> Create(TRequest request, CancellationToken cancellationToken)
    {
        var entity = await CreateEntityAsync(request, cancellationToken);
        repository.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await OnCreatedAsync(entity, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TEntity>> Update(string id, TRequest request, CancellationToken cancellationToken)
    {
        if (await repository.FindAsync(id, cancellationToken) is not { } entity) return NotFound();
        Apply(entity, request);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return entity;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TEntity>> Patch(string id, JsonObject changes, CancellationToken cancellationToken)
    {
        if (await repository.FindAsync(id, cancellationToken) is not { } entity) return NotFound();

        // JSON merge patch over the current write model, so only request fields can ever be changed.
        var merged = JsonSerializer.SerializeToNode(TRequest.FromEntity(entity), JsonOptions)!.AsObject();
        foreach (var (name, value) in changes) merged[name] = value?.DeepClone();
        var request = merged.Deserialize<TRequest>(JsonOptions)!;

        return TryValidateModel(request) ? await Update(id, request, cancellationToken) : ValidationProblem(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        if (await repository.FindAsync(id, cancellationToken) is not { } entity) return NotFound();
        repository.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    protected virtual Task<TEntity> CreateEntityAsync(TRequest request, CancellationToken cancellationToken)
        => Task.FromResult(request.ToEntity());

    protected virtual void Apply(TEntity entity, TRequest request) => request.ApplyTo(entity);

    /// <summary>Observer hook raised after a resource is persisted (e.g. real-time push through SignalR).</summary>
    protected virtual Task OnCreatedAsync(TEntity entity, CancellationToken cancellationToken) => Task.CompletedTask;

    private JsonSerializerOptions JsonOptions
        => HttpContext.RequestServices.GetRequiredService<IOptions<JsonOptions>>().Value.JsonSerializerOptions;
}
