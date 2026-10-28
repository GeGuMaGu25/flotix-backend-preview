using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

/// <summary>
///     Restricts a controller or action to the given roles; enforced by the request authorization middleware.
/// </summary>
/// <param name="roles">The roles allowed to access the endpoint; empty means any authenticated user.</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute(params Role[] roles) : Attribute
{
    /// <summary>
    ///     Gets the roles allowed to access the endpoint.
    /// </summary>
    public IReadOnlyList<Role> Roles { get; } = roles;
}
