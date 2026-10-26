namespace Flotix.Api.Shared.Domain;

/// <summary>Base type of every aggregate root: identity and creation timestamp.</summary>
public abstract class Entity(string? id = null)
{
    public string Id { get; private set; } = id ?? Guid.CreateVersion7().ToString();

    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
}
