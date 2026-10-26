using System.ComponentModel.DataAnnotations;
using Flotix.Api.Iam.Domain;
using Flotix.Api.Shared.Domain;
using Flotix.Api.Shared.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Flotix.Api.Iam.Web;

public sealed record UserRequest(
    [StringLength(120, MinimumLength = 1)] string Name,
    [EmailAddress, StringLength(254)] string Email,
    Role Role,
    [StringLength(128, MinimumLength = 8)] string? Password = null,
    [StringLength(20)] string? LicenseNumber = null,
    DateOnly? LicenseExpiry = null,
    [StringLength(120)] string? CompanyName = null,
    [StringLength(120)] string? WorkshopName = null) : IResourceRequest<UserRequest, User>
{
    public User ToEntity() => new(Name, Email, Role, LicenseNumber, LicenseExpiry, CompanyName, WorkshopName);

    public void ApplyTo(User user) => user.Update(Name, Email, Role, LicenseNumber, LicenseExpiry, CompanyName, WorkshopName);

    public static UserRequest FromEntity(User user)
        => new(user.Name, user.Email, user.Role, null, user.LicenseNumber, user.LicenseExpiry, user.CompanyName, user.WorkshopName);
}

public sealed class UsersController(IRepository<User> users, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
    : CrudController<User, UserRequest>(users, unitOfWork)
{
    /// <summary>Sign-up: stays public when the IAM middleware is enabled.</summary>
    [AllowAnonymous]
    public override Task<ActionResult<User>> Create(UserRequest request, CancellationToken cancellationToken)
        => base.Create(request, cancellationToken);

    protected override async Task<User> CreateEntityAsync(UserRequest request, CancellationToken cancellationToken)
    {
        if (request.Password is null) throw new DomainException("A password is required.");
        var user = await base.CreateEntityAsync(request, cancellationToken);
        user.ChangePasswordHash(passwordHasher.HashPassword(user, request.Password));
        return user;
    }

    protected override void Apply(User user, UserRequest request)
    {
        base.Apply(user, request);
        if (request.Password is not null) user.ChangePasswordHash(passwordHasher.HashPassword(user, request.Password));
    }
}
