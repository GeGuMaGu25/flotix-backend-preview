using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Queries;

/// <summary>
///     Query to get all users by role
/// </summary>
/// <param name="Role">The role to search</param>
public record GetAllUsersByRoleQuery(Role Role);
