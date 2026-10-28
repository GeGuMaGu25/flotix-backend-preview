using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Application.Services;
using FlotixPlatform.API.IAM.Domain.Model.Queries;
using FlotixPlatform.API.IAM.Domain.Repositories;

namespace FlotixPlatform.API.IAM.Application.Internal.QueryServices;

/// <summary>
///     Application service for querying users.
/// </summary>
/// <param name="userRepository">Repository for accessing user data.</param>
public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query,
        CancellationToken cancellationToken = default) =>
        await userRepository.ListAsync(cancellationToken);

    /// <inheritdoc />
    public async Task<IEnumerable<User>> Handle(GetAllUsersByRoleQuery query,
        CancellationToken cancellationToken = default) =>
        await userRepository.FindByRoleTypeAsync(query.RoleType, cancellationToken);

    /// <inheritdoc />
    public async Task<User?> Handle(GetUserByIdQuery query,
        CancellationToken cancellationToken = default) =>
        await userRepository.FindByIdAsync(query.Id, cancellationToken);

    /// <inheritdoc />
    public async Task<User?> Handle(GetUserByEmailQuery query,
        CancellationToken cancellationToken = default) =>
        await userRepository.FindByEmailAsync(query.Email, cancellationToken);
}
