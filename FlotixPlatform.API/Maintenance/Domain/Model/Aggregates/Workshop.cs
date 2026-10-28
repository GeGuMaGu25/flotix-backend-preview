using FlotixPlatform.API.Maintenance.Domain.Model.Commands;
using FlotixPlatform.API.Maintenance.Domain.Model.ValueObjects;

namespace FlotixPlatform.API.Maintenance.Domain.Model.Aggregates;

/// <summary>
///     Represents a mechanic workshop where vehicles can be repaired.
/// </summary>
/// <remarks>
///     This aggregate root is created from a CreateWorkshopCommand and updated through an UpdateWorkshopCommand.
///     Audit timestamps (CreatedAt, UpdatedAt) are managed by the persistence layer.
/// </remarks>
public partial class Workshop
{
    /// <summary>
    ///     Protected parameterless constructor for EF Core.
    /// </summary>
    protected Workshop()
    {
        City = string.Empty;
        District = string.Empty;
        Address = string.Empty;
        Phone = null!;
        Tags = [];
    }

    /// <summary>
    ///     Creates a new Workshop from a creation command.
    /// </summary>
    /// <param name="command">The CreateWorkshopCommand command</param>
    /// <exception cref="ArgumentNullException">Thrown when the command is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public Workshop(CreateWorkshopCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentOutOfRangeException.ThrowIfLessThan(command.MechanicId, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.City);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.City.Length, 80);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.District);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.District.Length, 80);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Address);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Address.Length, 200);
        ArgumentNullException.ThrowIfNull(command.Phone);
        ArgumentOutOfRangeException.ThrowIfNegative(command.Rating);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Rating, 5m);
        ArgumentNullException.ThrowIfNull(command.Tags);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Tags.Count, 20);

        MechanicId = command.MechanicId;
        City = command.City;
        District = command.District;
        Address = command.Address;
        Phone = command.Phone;
        Rating = command.Rating;
        Tags = command.Tags;
    }

    /// <summary>
    ///     Gets the server-generated identifier for this workshop.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the identifier of the mechanic user.
    /// </summary>
    public int MechanicId { get; private set; }

    /// <summary>
    ///     Gets the city where the workshop is located.
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    ///     Gets the district where the workshop is located.
    /// </summary>
    public string District { get; private set; }

    /// <summary>
    ///     Gets the street address of the workshop.
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    ///     Gets the contact phone number of the workshop.
    /// </summary>
    public PhoneNumber Phone { get; private set; }

    /// <summary>
    ///     Gets the rating of the workshop, from 0 to 5.
    /// </summary>
    public decimal Rating { get; private set; }

    /// <summary>
    ///     Gets the services offered by the workshop.
    /// </summary>
    public List<string> Tags { get; private set; }

    /// <summary>
    ///     Updates the workshop information from an update command.
    /// </summary>
    /// <param name="command">The UpdateWorkshopCommand command</param>
    /// <exception cref="ArgumentException">Thrown when the command contains invalid values.</exception>
    public void UpdateInformation(UpdateWorkshopCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.City);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.City.Length, 80);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.District);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.District.Length, 80);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.Address);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Address.Length, 200);
        ArgumentNullException.ThrowIfNull(command.Phone);
        ArgumentOutOfRangeException.ThrowIfNegative(command.Rating);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Rating, 5m);
        ArgumentNullException.ThrowIfNull(command.Tags);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(command.Tags.Count, 20);

        City = command.City;
        District = command.District;
        Address = command.Address;
        Phone = command.Phone;
        Rating = command.Rating;
        Tags = command.Tags;
    }
}
