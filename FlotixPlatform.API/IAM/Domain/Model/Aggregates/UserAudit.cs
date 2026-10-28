using FlotixPlatform.API.Shared.Domain.Model;

namespace FlotixPlatform.API.IAM.Domain.Model.Aggregates;

/// <summary>
///     Audit extension for User aggregate.
/// </summary>
/// <remarks>
///     This partial class extends User with audit trail properties.
///     CreatedAt and UpdatedAt are automatically managed by the persistence layer
///     via the AuditableEntityInterceptor in the Shared bounded context.
/// </remarks>
public partial class User : IAuditableEntity
{
    /// <summary>
    ///     Gets the timestamp when this user was created.
    /// </summary>
    /// <remarks>
    ///     Automatically set once by the persistence layer on first insert. Maps to column: created_at.
    /// </remarks>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets the timestamp when this user was last updated.
    /// </summary>
    /// <remarks>
    ///     Automatically refreshed by the persistence layer on every save. Maps to column: updated_at.
    /// </remarks>
    public DateTimeOffset? UpdatedAt { get; set; }
}
