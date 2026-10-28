using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Application.Errors;
using FlotixPlatform.API.Shared.Application.Patterns;

namespace FlotixPlatform.API.IAM.Application.Services;

/// <summary>
///     Command service interface for user operations.
/// </summary>
/// <remarks>
///     This interface defines the contract for signing up, signing in and managing user profiles.
/// </remarks>
public interface IUserCommandService
{
    /// <summary>
    ///     Handle the sign-up command.
    /// </summary>
    /// <param name="command">SignUpCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the created User on success, or the error reason on failure.
    /// </returns>
    Task<Result<User, SignUpError>> Handle(SignUpCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the sign-in command.
    /// </summary>
    /// <param name="command">SignInCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the authenticated User and its bearer token on success, or the error reason on failure.
    /// </returns>
    Task<Result<(User User, string Token), SignInError>> Handle(SignInCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the update user profile command.
    /// </summary>
    /// <param name="command">UpdateUserProfileCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the updated User on success, or the error reason on failure.
    /// </returns>
    Task<Result<User, UpdateUserProfileError>> Handle(UpdateUserProfileCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handle the delete user command.
    /// </summary>
    /// <param name="command">DeleteUserCommand command</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    ///     A Result containing the deleted User on success, or the error reason on failure.
    /// </returns>
    Task<Result<User, DeleteUserError>> Handle(DeleteUserCommand command, CancellationToken cancellationToken = default);
}
