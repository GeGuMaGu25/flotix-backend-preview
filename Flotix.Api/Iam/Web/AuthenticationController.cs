using System.ComponentModel.DataAnnotations;
using Flotix.Api.Iam.Domain;
using Flotix.Api.Iam.Security;
using Flotix.Api.Shared.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flotix.Api.Iam.Web;

public sealed record SignInRequest([EmailAddress] string Email, [StringLength(128, MinimumLength = 1)] string Password);

public sealed record SignInResponse(User User, string Token);

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public sealed class AuthenticationController(IRepository<User> users, IPasswordHasher<User> passwordHasher, ITokenService tokens) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<SignInResponse>> SignIn(SignInRequest request, CancellationToken cancellationToken)
    {
        var email = Domain.User.NormalizeEmail(request.Email);
        var user = (await users.ListAsync(u => u.Email == email, cancellationToken)).SingleOrDefault();

        if (user is null || passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Unauthorized();

        return new SignInResponse(user, tokens.Issue(user));
    }
}
