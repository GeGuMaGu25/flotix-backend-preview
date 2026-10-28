namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to delete a workshop
/// </summary>
/// <param name="WorkshopId">The identifier of the workshop to delete</param>
public record DeleteWorkshopCommand(int WorkshopId);
