namespace FlotixPlatform.API.IAM.Domain.Model.Queries;

/// <summary>
///     Query to get a user by ID
/// </summary>
/// <param name="Id">The user ID to search</param>
public record GetUserByIdQuery(Guid Id);
