namespace FlotixPlatform.API.IAM.Domain.Model.Commands;

/// <summary>
///     Command to delete a user
/// </summary>
/// <param name="UserId">The identifier of the user to delete</param>
public record DeleteUserCommand(Guid UserId);
