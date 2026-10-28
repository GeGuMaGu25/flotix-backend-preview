using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Domain.Model.Queries;

namespace FlotixPlatform.API.IAM.Application.Services;

/// <summary>
///     Query service interface for user operations.
/// </summary>
public interface IUserQueryService
{
	/// <summary>
	///     Handle the GetAllUsersQuery.
	/// </summary>
	/// <param name="query">The GetAllUsersQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the User objects</returns>
	Task<IEnumerable<User>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetAllUsersByRoleQuery.
	/// </summary>
	/// <param name="query">The GetAllUsersByRoleQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>An IEnumerable containing the User objects</returns>
	Task<IEnumerable<User>> Handle(GetAllUsersByRoleQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetUserByIdQuery.
	/// </summary>
	/// <param name="query">The GetUserByIdQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The User object if found, or null otherwise</returns>
	Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken = default);

	/// <summary>
	///     Handle the GetUserByEmailQuery.
	/// </summary>
	/// <param name="query">The GetUserByEmailQuery query</param>
	/// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
	/// <returns>The User object if found, or null otherwise</returns>
	Task<User?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken = default);
}
