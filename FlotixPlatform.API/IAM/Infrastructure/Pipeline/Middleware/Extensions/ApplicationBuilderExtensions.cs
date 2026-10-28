using FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Components;

namespace FlotixPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
///     Extension methods to plug the IAM request authorization into the HTTP pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    ///     Adds the <see cref="RequestAuthorizationMiddleware"/> to the HTTP pipeline.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestAuthorizationMiddleware>();
    }
}
