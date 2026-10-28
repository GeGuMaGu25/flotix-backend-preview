using System.ComponentModel.DataAnnotations;

namespace FlotixPlatform.API.IAM.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data required to sign in.
/// </summary>
/// <param name="Email">The email address of the user (required)</param>
/// <param name="Password">The password of the user (required)</param>
public record SignInResource(
    [Required(ErrorMessage = "RequiredEmail")]
    [EmailAddress(ErrorMessage = "EmailAddressField")]
    string Email,
    [Required(ErrorMessage = "RequiredPassword")]
    string Password);
