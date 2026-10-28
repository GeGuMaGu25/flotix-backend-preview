using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Commands;

/// <summary>
///     Command to sign in a user
/// </summary>
/// <param name="Email">The email address of the user.</param>
/// <param name="Password">The plain password of the user.</param>
public record SignInCommand(EmailAddress Email, string Password);
