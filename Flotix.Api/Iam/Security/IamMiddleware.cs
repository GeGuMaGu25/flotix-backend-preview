using Microsoft.AspNetCore.Authorization;

namespace Flotix.Api.Iam.Security;

/// <summary>
/// IAM request filter: resolves the caller from the bearer token and protects every endpoint
/// that is not marked <see cref="AllowAnonymousAttribute"/>; <see cref="AllowRolesAttribute"/> narrows it by role.
/// Disconnected by default — see <c>Program.cs</c> to enable it.
/// </summary>
public sealed class IamMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ITokenService tokens)
    {
        var header = context.Request.Headers.Authorization.ToString();
        var token = header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)
            ? header["Bearer ".Length..]
            // Browsers cannot send headers on WebSockets, so SignalR sends the token in the query string.
            : context.Request.Path.StartsWithSegments("/hubs") ? context.Request.Query["access_token"].ToString() : null;

        if (tokens.Validate(token) is { } principal) context.User = principal;

        var metadata = context.GetEndpoint()?.Metadata;
        if (metadata?.GetMetadata<IAllowAnonymous>() is null)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (metadata?.GetMetadata<AllowRolesAttribute>() is { } allowed && !allowed.Roles.Any(role => context.User.IsInRole(role.ToString())))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }
        }

        await next(context);
    }
}
