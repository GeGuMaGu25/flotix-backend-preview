namespace FlotixPlatform.API.Maintenance.Interfaces.Rest.Resources;

/// <summary>
///     Represents the data provided by the server about a workshop.
/// </summary>
/// <param name="Id">The server-generated ID of the workshop</param>
/// <param name="MechanicId">The identifier of the mechanic user.</param>
/// <param name="City">The city where the workshop is located.</param>
/// <param name="District">The district where the workshop is located.</param>
/// <param name="Address">The street address of the workshop.</param>
/// <param name="Phone">The contact phone number of the workshop.</param>
/// <param name="Rating">The rating of the workshop, from 0 to 5.</param>
/// <param name="Tags">The services offered by the workshop.</param>
public record WorkshopResource(
    int Id,
    int MechanicId,
    string City,
    string District,
    string Address,
    string Phone,
    decimal Rating,
    List<string> Tags);
