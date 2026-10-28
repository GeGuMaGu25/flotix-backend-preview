namespace FlotixPlatform.API.Maintenance.Domain.Model.Queries;

/// <summary>
///     Query to get a workshop by ID
/// </summary>
/// <param name="Id">The workshop ID to search</param>
public record GetWorkshopByIdQuery(int Id);
