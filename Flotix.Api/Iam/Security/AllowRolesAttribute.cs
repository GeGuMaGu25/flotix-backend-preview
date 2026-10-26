using Flotix.Api.Iam.Domain;

namespace Flotix.Api.Iam.Security;

/// <summary>Restricts a controller or action to the given roles; enforced by <see cref="IamMiddleware"/>.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AllowRolesAttribute(params Role[] roles) : Attribute
{
    public IReadOnlyList<Role> Roles { get; } = roles;
}
