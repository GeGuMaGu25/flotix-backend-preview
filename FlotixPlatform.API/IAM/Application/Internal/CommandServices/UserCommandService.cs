using FlotixPlatform.API.IAM.Domain.Model.Aggregates;
using FlotixPlatform.API.IAM.Application.Errors;
using FlotixPlatform.API.IAM.Application.Internal.OutboundServices;
using FlotixPlatform.API.IAM.Application.Services;
using FlotixPlatform.API.IAM.Domain.Model.Commands;
using FlotixPlatform.API.IAM.Domain.Repositories;
using FlotixPlatform.API.Shared.Domain.Repositories;
using FlotixPlatform.API.Shared.Application.Patterns;
using Microsoft.EntityFrameworkCore;

namespace FlotixPlatform.API.IAM.Application.Internal.CommandServices;

/// <summary>
///     Application service for handling user commands.
/// </summary>
/// <remarks>
///     This service acts as the command handler for signing up, signing in and managing user profiles.
///     Passwords are hashed through the hashing service and tokens are issued through the token service.
///     Logs warnings for rejected commands and errors for persistence failures, never logging passwords.
/// </remarks>
/// <param name="userRepository">Repository for accessing user data.</param>
/// <param name="hashingService">Outbound service for hashing and verifying passwords.</param>
/// <param name="tokenService">Outbound service for issuing bearer tokens.</param>
/// <param name="unitOfWork">Unit of work for managing transaction scope.</param>
/// <param name="logger">Logger for diagnostic and error reporting.</param>
public class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    ILogger<UserCommandService> logger)
    : IUserCommandService
{
    /// <inheritdoc />
    public async Task<Result<User, SignUpError>> Handle(SignUpCommand command,
        CancellationToken cancellationToken = default)
    {
        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
        {
            logger.LogWarning("Duplicate user rejected for Email {Email}", command.Email);
            return new Result<User, SignUpError>.Failure(SignUpError.DuplicateUserEmail);
        }

        try
        {
            var user = User.Create(command, hashingService.HashPassword(command.Password));
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<User, SignUpError>.Success(user);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Business rule rejected signing up user");
            return new Result<User, SignUpError>.Failure(SignUpError.DriverLicenseRequired);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while signing up user");
            return new Result<User, SignUpError>.Failure(SignUpError.InvalidUser);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            logger.LogWarning(ex, "Duplicate key violation signing up user");
            return new Result<User, SignUpError>.Failure(SignUpError.DuplicateUserEmail);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed signing up user");
            return new Result<User, SignUpError>.Failure(SignUpError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error signing up user");
            return new Result<User, SignUpError>.Failure(SignUpError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<(User User, string Token), SignInError>> Handle(SignInCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
            if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            {
                logger.LogWarning("Invalid credentials for Email {Email}", command.Email);
                return new Result<(User User, string Token), SignInError>.Failure(SignInError.InvalidCredentials);
            }

            return new Result<(User User, string Token), SignInError>.Success((user, tokenService.GenerateToken(user)));
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error signing in user");
            return new Result<(User User, string Token), SignInError>.Failure(SignInError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<User, UpdateUserProfileError>> Handle(UpdateUserProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return new Result<User, UpdateUserProfileError>.Failure(UpdateUserProfileError.UserNotFound);

        try
        {
            user.UpdateProfile(command);
            userRepository.Update(user);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<User, UpdateUserProfileError>.Success(user);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "Business rule rejected updating user profile");
            return new Result<User, UpdateUserProfileError>.Failure(UpdateUserProfileError.DriverLicenseRequired);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while updating user profile");
            return new Result<User, UpdateUserProfileError>.Failure(UpdateUserProfileError.InvalidUser);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed updating user profile");
            return new Result<User, UpdateUserProfileError>.Failure(UpdateUserProfileError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error updating user profile");
            return new Result<User, UpdateUserProfileError>.Failure(UpdateUserProfileError.UnexpectedError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<User, DeleteUserError>> Handle(DeleteUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return new Result<User, DeleteUserError>.Failure(DeleteUserError.UserNotFound);

        try
        {
            userRepository.Remove(user);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<User, DeleteUserError>.Success(user);
        }
        catch (DbUpdateException ex) when (IsForeignKeyViolation(ex))
        {
            logger.LogWarning(ex, "Foreign key violation deleting user");
            return new Result<User, DeleteUserError>.Failure(DeleteUserError.UserHasDependents);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed deleting user");
            return new Result<User, DeleteUserError>.Failure(DeleteUserError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error deleting user");
            return new Result<User, DeleteUserError>.Failure(DeleteUserError.UnexpectedError);
        }
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a duplicate key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL unique violation (SQLSTATE 23505), false otherwise.</returns>
    private static bool IsDuplicateKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23505")
                return true;
        }
        return false;
    }

    /// <summary>
    /// Determines whether a DbUpdateException represents a foreign key constraint violation.
    /// </summary>
    /// <param name="exception">The exception to inspect.</param>
    /// <returns>True if the exception is due to a PostgreSQL foreign key violation (SQLSTATE 23503), false otherwise.</returns>
    private static bool IsForeignKeyViolation(DbUpdateException exception)
    {
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.Equals(current.GetType().Name, "PostgresException", StringComparison.Ordinal)) continue;
            var sqlStateProperty = current.GetType().GetProperty("SqlState");
            if (sqlStateProperty?.GetValue(current) is "23503")
                return true;
        }
        return false;
    }
}
