namespace FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

/// <summary>
///     Marks a controller or action as public: the request authorization middleware lets it through without a token.
/// </summary>
/// <remarks>
///     It implements ASP.NET Core's <see cref="Microsoft.AspNetCore.Authorization.IAllowAnonymous"/> marker, so
///     endpoints configured with <c>.AllowAnonymous()</c> (e.g. OpenAPI and Scalar) are treated the same way.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute, Microsoft.AspNetCore.Authorization.IAllowAnonymous;
