using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;
using FlotixPlatform.API.Shared.Domain.Repositories;

namespace FlotixPlatform.API.IAM.Domain.Repositories;

/// <summary>
///     The user repository contract.
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    ///     Find a user by email address
    /// </summary>
    /// <param name="email">The email address to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     The user object if found, or null otherwise.
    /// </returns>
    Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Check whether a user with the given email address exists
    /// </summary>
    /// <param name="email">The email address to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     True if a user with the email address exists, false otherwise.
    /// </returns>
    Task<bool> ExistsByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Find users by role type
    /// </summary>
    /// <param name="roleType">The role to search.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     An Enumerable containing the user objects if found, or empty otherwise.
    /// </returns>
    Task<IEnumerable<User>> FindByRoleTypeAsync(Role roleType, CancellationToken cancellationToken = default);
}
