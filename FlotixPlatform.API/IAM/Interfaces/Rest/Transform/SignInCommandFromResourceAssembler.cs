using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a SignInCommand from a SignInResource.
/// </summary>
public static class SignInCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a SignInCommand from a SignInResource.
    /// </summary>
    /// <param name="resource">The SignInResource resource</param>
    /// <returns>
    ///     A SignInCommand assembled from the SignInResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the email address is not valid.</exception>
    public static SignInCommand ToCommandFromResource(SignInResource resource) =>
        new(new EmailAddress(resource.Email), resource.Password);
}
