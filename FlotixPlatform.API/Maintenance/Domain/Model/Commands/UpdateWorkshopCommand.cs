using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Commands;

/// <summary>
///     Command to update a workshop
/// </summary>
/// <param name="WorkshopId">The identifier of the workshop to update</param>
/// <param name="City">The city where the workshop is located.</param>
/// <param name="District">The district where the workshop is located.</param>
/// <param name="Address">The street address of the workshop.</param>
/// <param name="Phone">The contact phone number of the workshop.</param>
/// <param name="Rating">The rating of the workshop, from 0 to 5.</param>
/// <param name="Tags">The services offered by the workshop.</param>
public record UpdateWorkshopCommand(int WorkshopId, string City, string District, string Address, PhoneNumber Phone, decimal Rating, List<string> Tags);
