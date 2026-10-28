using FlotixPlatform.API.IAM.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.IAM.Domain.Model.Queries;

/// <summary>
///     Query to get all users by role
/// </summary>
/// <param name="RoleType">The role to search</param>
public record GetAllUsersByRoleQuery(Role RoleType);
