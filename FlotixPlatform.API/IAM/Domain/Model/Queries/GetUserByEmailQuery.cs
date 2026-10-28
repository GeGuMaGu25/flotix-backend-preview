using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Queries;

/// <summary>
///     Query to get a user by email address
/// </summary>
/// <param name="Email">The email address to search</param>
public record GetUserByEmailQuery(EmailAddress Email);
