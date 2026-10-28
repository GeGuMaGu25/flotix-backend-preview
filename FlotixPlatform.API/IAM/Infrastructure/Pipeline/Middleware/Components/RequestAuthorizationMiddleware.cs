using System.Security.Claims;
using FlotixPlatform.API.IAM.Application.Internal.OutboundServices;
using FlotixPlatform.API.IAM.Application.Services;
using FlotixPlatform.API.IAM.Domain.Model.Queries;
using FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Request authorization middleware of the IAM bounded context.
/// </summary>
/// <remarks>
///     Every endpoint not marked as anonymous requires a valid bearer token whose user still exists;
///     <see cref="AuthorizeAttribute"/> narrows the access by role. The authenticated user is stored in
///     <c>HttpContext.Items["User"]</c> and as the request principal (used by SignalR to target users).
///     Browsers cannot send headers on WebSockets, so hubs (<c>/hubs/*</c>) accept the token in the
///     <c>access_token</c> query parameter.
/// </remarks>
/// <param name="next">The next middleware in the pipeline.</param>
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    /// <summary>
    ///     Authorizes the current request.
    /// </summary>
    /// <param name="context">The HTTP context of the request.</param>
    /// <param name="userQueryService">The user query service used to load the token owner.</param>
    /// <param name="tokenService">The token service used to validate the bearer token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context, IUserQueryService userQueryService, ITokenService tokenService)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Authorization.IAllowAnonymous>() is not null)
        {
            await next(context);
            return;
        }

        var token = GetToken(context.Request);
        var userId = token is null ? null : tokenService.ValidateToken(token);
        var user = userId is null
            ? null
            : await userQueryService.Handle(new GetUserByIdQuery(userId.Value), context.RequestAborted);
        if (user is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() is { Roles.Count: > 0 } authorize &&
            !authorize.Roles.Contains(user.RoleType))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        context.Items["User"] = user;
        context.User = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.RoleType.ToString())
        ], "Bearer"));

        await next(context);
    }

    private static string? GetToken(HttpRequest request)
    {
        var authorization = request.Headers.Authorization.ToString();
        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return authorization["Bearer ".Length..].Trim();

        return request.Path.StartsWithSegments("/hubs") && request.Query.TryGetValue("access_token", out var accessToken)
            ? accessToken.ToString()
            : null;
    }
}
