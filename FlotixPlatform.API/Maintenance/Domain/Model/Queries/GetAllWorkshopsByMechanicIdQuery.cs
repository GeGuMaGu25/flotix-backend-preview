namespace FlotixPlatform.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get all workshops by mechanic ID
/// </summary>
/// <param name="MechanicId">The mechanic ID to search</param>
public record GetAllWorkshopsByMechanicIdQuery(int MechanicId);
