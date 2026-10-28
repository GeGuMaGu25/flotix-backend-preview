using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Transform;

/// <summary>
///     Assembles a SignUpCommand from a SignUpResource.
/// </summary>
public static class SignUpCommandFromResourceAssembler
{
    /// <summary>
    ///     Assembles a SignUpCommand from a SignUpResource.
    /// </summary>
    /// <param name="resource">The SignUpResource resource</param>
    /// <returns>
    ///     A SignUpCommand assembled from the SignUpResource
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the email address is not valid.</exception>
    public static SignUpCommand ToCommandFromResource(SignUpResource resource) =>
        new(new EmailAddress(resource.Email), resource.Password, resource.Role, resource.FullName,
            resource.LicenseNumber, resource.LicenseExpiry, resource.CompanyName, resource.WorkshopName);
}
